FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

# Copy all .csproj files and restore as distinct layers
COPY auction-app.sln auction-app.sln
COPY AuctionService/AuctionService.csproj AuctionService/AuctionService.csproj
COPY SearchService/SearchService.csproj SearchService/SearchService.csproj
COPY IdentityService/IdentityService.csproj IdentityService/IdentityService.csproj
COPY Gateway/Gateway.csproj Gateway/Gateway.csproj
COPY BidService/BidService.csproj BidService/BidService.csproj

# Restore package dependencies
RUN dotnet restore auction-app.sln

# Copy the app folders over
COPY SearchService/ SearchService/
WORKDIR /app/SearchService
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "SearchService.dll" ]