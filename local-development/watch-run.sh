#!/bin/bash

KAFKA_BOOTSTRAP_SERVERS=localhost:9092 \
KAFKA_GROUP_ID=kubernetes-consumer \
KAFKA_ENABLE_AUTO_COMMIT=false \
dotnet watch --project ./../src/IAMRoleService.WebApi/IAMRoleService.WebApi.csproj run
