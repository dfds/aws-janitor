#!/bin/bash
#
# build.sh(1)
#

[[ -n $DEBUG ]] && set -x
set -eu -o pipefail

# build parameters
readonly REGION=${AWS_DEFAULT_REGION:-"eu-central-1"}
readonly IMAGE_NAME='aws-janitor'
readonly BUILD_NUMBER=${1:-"N/A"}
readonly BUILD_SOURCES_DIRECTORY=${2:-${PWD}}
readonly SERVICE_NAME="AwsJanitor"

clean_output_folder() {
    rm -Rf ./output
    mkdir output
}

restore_dependencies() {
    echo "Restoring dependencies"
    dotnet restore ${SERVICE_NAME}.sln
}

run_tests() {
    echo "Running tests..."
    dotnet build -c Release ${SERVICE_NAME}.sln

    MSYS_NO_PATHCONV=1 dotnet test \
        --logger:"trx;LogFileName=testresults.trx" \
        AwsJanitor.WebApi.Tests/AwsJanitor.WebApi.Tests.csproj \
        /p:CollectCoverage=true \
        /p:CoverletOutputFormat=cobertura \
        '/p:Include="[AwsJanitor.WebApi]*"'

    mv ./AwsJanitor.WebApi.Tests/coverage.cobertura.xml "${BUILD_SOURCES_DIRECTORY}/output/"
    mv ./AwsJanitor.WebApi.Tests/TestResults/testresults.trx "${BUILD_SOURCES_DIRECTORY}/output/"
}

publish_binaries() {
    echo "Publishing binaries..."
    dotnet publish -c Release -o ${BUILD_SOURCES_DIRECTORY}/output/app AwsJanitor.WebApi/AwsJanitor.WebApi.csproj
}

build_container_image() {
    echo "Building container image..."
    docker build -t ${IMAGE_NAME} .
}

push_container_image() {
    echo "Login to docker..."
    $(aws ecr get-login --no-include-email)

    account_id=$(aws sts get-caller-identity --output text --query 'Account')
    image_name="${account_id}.dkr.ecr.${REGION}.amazonaws.com/ded/${IMAGE_NAME}:${BUILD_NUMBER}"

    echo "Tagging container image..."
    docker tag ${IMAGE_NAME}:latest ${image_name}

    echo "Pushing container image to ECR..."
    docker push ${image_name}
}

clean_output_folder

cd ./src

restore_dependencies
run_tests
publish_binaries

cd ..

build_container_image

if [[ "${BUILD_NUMBER}" != "N/A" ]]; then
    push_container_image
fi