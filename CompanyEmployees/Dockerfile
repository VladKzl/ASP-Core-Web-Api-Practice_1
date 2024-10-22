FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /home/app
COPY ./bin/Release/net8.0/publish .
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENTRYPOINT ["dotnet", "CompanyEmployees.dll"]
