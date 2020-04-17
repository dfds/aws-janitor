using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Controllers;
using AwsJanitor.WebApi.Domain.Events;
using AwsJanitor.WebApi.EventHandlers;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Swashbuckle.AspNetCore.Swagger;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AwsJanitor.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "IAM Role Service",
                    Version = "v1.0.0"
                });

                c.EnableAnnotations();
            });


            RolesServicesConfiguration.ConfigureServices(services);

            ParameterStoreServicesConfiguration.ConfigureServices(Configuration, services);

            services.AddHostedService<MetricHostedService>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            ConfigureDomainEvents(services);

            services.AddHostedService<KafkaConsumerHostedService>();
        }


        private static void ConfigureDomainEvents(IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();
            services.AddSingleton(eventRegistry);
            services.AddTransient<IEventHandler<ContextAddedToCapabilityDomainEvent>, ContextAddedToCapabilityEventHandler>();


            services.AddTransient<KafkaConsumerFactory.KafkaConfiguration>();
            services.AddTransient<KafkaConsumerFactory>();

            var serviceProvider = services.BuildServiceProvider();

            eventRegistry
                .Register<ContextAddedToCapabilityDomainEvent>(
                    eventTypeName: "capability_created",
                    topicName: "build.capabilities",
                    eventHandler: serviceProvider.GetRequiredService<IEventHandler<ContextAddedToCapabilityDomainEvent>>());

            services.AddTransient<IEventDispatcher, EventDispatcher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = MyPrometheusStuff.WriteResponseAsync
            });

            app.UseMvc();

            app.UseSwagger(x =>
            {
                const string basePath = "/api";

                x.PreSerializeFilters.Add((doc, req) => { doc.BasePath = basePath; });

                x.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var paths = new Dictionary<string, PathItem>();

                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(path.Key.Replace(basePath, ""), path.Value);
                    }

                    swaggerDoc.Paths = paths;
                });
            });

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "IAM Role Service API"); });
        }
    }

    public class MetricHostedService : IHostedService
    {
        private const string Host = "0.0.0.0";
        private const int Port = 8080;

        private IMetricServer _metricServer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Staring metric server on {Host}:{Port}");

            _metricServer = new KestrelMetricServer(Host, Port).Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (_metricServer)
            {
                Console.WriteLine("Shutting down metric server");
                await _metricServer.StopAsync();
                Console.WriteLine("Done shutting down metric server");
            }
        }
    }

    public static class MyPrometheusStuff
    {
        private const string HealthCheckLabelServiceName = "service";
        private const string HealthCheckLabelStatusName = "status";

        private static readonly Gauge HealthChecksDuration;
        private static readonly Gauge HealthChecksResult;

        static MyPrometheusStuff()
        {
            HealthChecksResult = Metrics.CreateGauge("healthcheck",
                "Shows health check status (status=unhealthy|degraded|healthy) 1 for triggered, otherwise 0",
                new GaugeConfiguration
                {
                    LabelNames = new[] {HealthCheckLabelServiceName, HealthCheckLabelStatusName},
                    SuppressInitialValue = false
                });

            HealthChecksDuration = Metrics.CreateGauge("healthcheck_duration_seconds",
                "Shows duration of the health check execution in seconds",
                new GaugeConfiguration
                {
                    LabelNames = new[] {HealthCheckLabelServiceName},
                    SuppressInitialValue = false
                });
        }

        public static Task WriteResponseAsync(HttpContext httpContext, HealthReport healthReport)
        {
            UpdateMetrics(healthReport);

            httpContext.Response.ContentType = "text/plain";
            return httpContext.Response.WriteAsync(healthReport.Status.ToString());
        }

        private static void UpdateMetrics(HealthReport report)
        {
            foreach (var (key, value) in report.Entries)
            {
                HealthChecksResult.Labels(key, "healthy").Set(value.Status == HealthStatus.Healthy ? 1 : 0);
                HealthChecksResult.Labels(key, "unhealthy").Set(value.Status == HealthStatus.Unhealthy ? 1 : 0);
                HealthChecksResult.Labels(key, "degraded").Set(value.Status == HealthStatus.Degraded ? 1 : 0);

                HealthChecksDuration.Labels(key).Set(value.Duration.TotalSeconds);
            }
        }
    }
}