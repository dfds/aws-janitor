# IAM Role Service
[![Build Status](https://dfds.visualstudio.com/DevelopmentExcellence/_apis/build/status/iamroleservice-ci)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=961)

A small service for creating IAM roles in AWS through an API.

## Development

### Prerequisites
- .NET Core 2.2 SDK ([download](https://dotnet.microsoft.com/download/dotnet-core/2.2))
- Docker (any relatively new version will do)

Other than the above prerequisites this application requires no additional 
special setup on your machine. Open the solution or root folder in your 
editor of choice and start cracking!

## Running the application locally
To run the application locally you have two options e.g. in a container or directly 
on your machine using dotnet cli.

### Environment variables
The application requires the following environment variables when it's running:

| Name | Description |
|------|-------------|
| AWS_ACCESS_KEY_ID | Key part of access key pair for a specific AWS account |
| AWS_SECRET_ACCESS_KEY | Secret part of access key pair for a specific AWS account |
| AWS_REGION | The region where the IAM roles are written to |

### Running in a container
To run the application you first need to execute a script located in the repository 
root. This will restore any dependencies and build both the application and also a 
container image using Docker. Run the following on your command line in the repository 
root:

```shell
$ ./pipeline.sh
```

__Please note:__ the script above is a linux bash script and needs a bash runtime to execute.

Now you should be able to start a container by running the following on your command line:

```shell
$ docker run -it --rm -p 8080:80 iamroleservice
```

You should be able to navigate to `http://localhost:8080` in a browser.

__Please note:__ The url above might return `404 - not found` - instead try an endpoint 
that the application serves e.g. `/swagger`.

### Running with dotnet cli
To run the application on your local machine run the following on your command line in 
the `/src` folder in your repository:

```shell
$ dotnet build
```

Followed by:

```shell
$ dotnet run --project AwsJanitor.WebApi/AwsJanitor.WebApi.csproj
```

