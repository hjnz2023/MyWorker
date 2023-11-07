# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyWorker/MyWorker.csproj", "MyWorker/"]
RUN dotnet restore "MyWorker/MyWorker.csproj"
COPY . .
WORKDIR "/src/MyWorker"
RUN dotnet build "MyWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyWorker.csproj" -c Release -o /app/publish

FROM base AS final

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

COPY newrelic.config /usr/local/newrelic-dotnet-agent/newrelic.config

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/usr/local/newrelic-dotnet-agent/run.sh", "dotnet", "MyWorker.dll"]