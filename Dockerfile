FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-image
WORKDIR /home/app
COPY . .
RUN dotnet restore ./CompanyEmployees
RUN dotnet test ./Tests/Tests.csproj

WORKDIR /home/app/CompanyEmployees
RUN dotnet publish CompanyEmployees.csproj -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /publish
COPY --from=build-image /publish .

ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "CompanyEmployees.dll"]