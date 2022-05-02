FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env

# Build requires 3.1 SDK
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:4
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]

#RUN echo "deb http://dk.archive.ubuntu.com/ubuntu/ bionic main universe" >> /etc/apt/sources.list && apt-get update && apt-get -y install gcc-6 g++-6
RUN apt-get update && DEBIAN_FRONTEND='noninteractive' apt-get install -y python3 python3-distutils python3-setuptools python3-pip python3-opencv
COPY faceDetect.py /home/site/wwwroot
