FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./api.csproj .
RUN dotnet restore

# copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

#build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out ./
COPY --from=build-env /app/Modules/TodoListModule/Migrations ./
ENTRYPOINT ["dotnet", "./api.dll"]
