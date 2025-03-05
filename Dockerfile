FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0@sha256:89669b04fa9d98dc2f4519184af3b953e464c4f3ba0da1c3c42d822aa8b68def
# set timezone
ENV TZ="Asia/Taipei"
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "dc_sobel_haiyaa_dev.dll"]