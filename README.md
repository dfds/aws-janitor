# AWS Janitor
[![Build Status](https://dfds.visualstudio.com/DevelopmentExcellence/_apis/build/status/aws-janitor-ci)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=961)

A service taking care of our shared AWS account, applying our business logics to resources in the shared AWS account. The janitor is part of our self service eco-system and will act upon events raised by other entities in the self service system.

## Development

### Prerequisites

- .NET Core 2.2 SDK ([download](https://dotnet.microsoft.com/download/dotnet-core/2.2))
- Docker (any relatively new version will do)
- All scripts are written in bash. You can use gitbash on windows

Other than the above prerequisites this application requires no additional
special setup on your machine. Open the solution or root folder in your
editor of choice and start cracking!

## Access to AWS

The application operates on a AWS account and needs a set of AWS Credentials. The application respecs the [Default Credential Provider Chain](https://docs.aws.amazon.com/sdk-for-java/v1/developer-guide/credentials.html) and will use the credentials provided by logging with saml2aws.

## Running the application locally

The folder local-development contains bash scripts that enables you to run the application locally while developing.

- start-dependencies.sh starts a kafka cluster
- watch-run.sh starts the api project with environment variables set for local development and will rebuild on file change
- watch-run-unit-tests.sh runs unit tests and will rerun them on file change

### Environment variables

The application requires the following environment variables when running locally:

| Name | Description |
|------|-------------|
| AWS_REGION | The region the janitor will operate in |
| KUBERNETES_CLUSTER_NAME | The cluster the application is running in, this is used by the ParameterStore feature
| KAFKA_BOOTSTRAP_SERVERS | A list of host/port pairs to use for establishing the initial connection to the Kafka cluster.
| KAFKA_GROUP_ID | Id of the consumer group that the application will join. Only a single consumer in a group will read a message.
| KAFKA_ENABLE_AUTO_COMMIT | commit the Offset on Consumer fetches or manually.

### Kafka has its own subset set of environment variables

Thay can be found in `Infrastructure/Messaging/KafkaConsumerFactory.cs`

### Building & running in a container

To run the application you first need to execute a script located in the repository
root. This will restore any dependencies and build both the application and also a
container image using Docker. Run the following on your command line in the repository root:

```shell
./pipeline.sh
```

Now you should be able to start a container by running the following on your command line:

```shell
docker run -it --rm -p 8080:80 aws-janitor
```

You should be able to navigate to `http://localhost:8080` in a browser.

__Please note:__ The url above might return `404 - not found` - instead try an endpoint 
that the application serves e.g. `/swagger`.
