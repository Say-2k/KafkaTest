FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

EXPOSE 80

WORKDIR /app
COPY . ./
RUN dotnet restore

RUN dotnet build -c Release -o /app/build/
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:7.0

RUN apt-get update

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "KafkaConsumer.dll", "--urls", "http://0.0.0.0:5000"]
