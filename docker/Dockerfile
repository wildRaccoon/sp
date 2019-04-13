FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY dist/sp.auth.service/ ./
ENTRYPOINT ["dotnet", "sp.auth.service.dll"]