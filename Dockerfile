# Use the official .NET Core SDK image as a base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory in the container
# Set the working directory in the container
WORKDIR /app

# Copy the C# project file(s) and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project directory and build the application
COPY . ./

# Copy the SQLite database file from the "db" directory
COPY DB/shenkard.sqlite /app/DB/

RUN dotnet publish -c Release -o out

# Define the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port that your socket server listens on
EXPOSE 8888

# Command to run the application
ENTRYPOINT ["dotnet", "Server.dll"]
