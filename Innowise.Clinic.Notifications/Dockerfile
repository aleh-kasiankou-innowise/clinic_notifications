FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Innowise.Clinic.Notifications/NuGet.Config", "./"]
COPY ["Innowise.Clinic.Notifications/Innowise.Clinic.Notifications.csproj", "Innowise.Clinic.Notifications/"]
RUN dotnet restore "Innowise.Clinic.Notifications/Innowise.Clinic.Notifications.csproj"
COPY . .
WORKDIR "/src/Innowise.Clinic.Notifications"
RUN dotnet build "Innowise.Clinic.Notifications.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innowise.Clinic.Notifications.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Notifications.dll"]
