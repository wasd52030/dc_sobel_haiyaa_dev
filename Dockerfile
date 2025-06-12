FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
# set timezone
ENV TZ="Asia/Taipei"
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "dc_sobel_haiyaa_dev.dll"]