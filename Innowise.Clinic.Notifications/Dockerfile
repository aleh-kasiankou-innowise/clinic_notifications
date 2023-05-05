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

FROM base as withdependencies
WORKDIR /app
RUN apt-get update
RUN apt-get install -y wkhtmltopdf
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN apt-get install -y libc6-dev
RUN ln -s /usr/lib/libgdiplus.so/usr/lib/gdiplus.dll
RUN apt-get install wget
RUN wget -P . https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dll
RUN wget -P . https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dylib
RUN wget -P . https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.so

FROM withdependencies AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Notifications.dll"]
