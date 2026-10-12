# DockerDemo

A small ASP.NET Core MVC application targeting .NET 10. It displays the configured `APP_ENVIRONMENT` value and provides a simple example of packaging an existing .NET application in a Docker image.

The demo workflow is:

**Existing .NET app → Dockerfile → Docker image → Docker container → Running application**

## Requirements

- .NET 10 SDK for running the app locally
- Docker Desktop (macOS or Windows) or Docker Engine (Linux) for building and running the container
- A web browser

Verify the .NET SDK installation with:

```bash
dotnet --version
```

The reported version should be `10.x`.

Verify Docker is available and its daemon is running with:

```bash
docker version
```

The command should show both a Client and a Server version. On macOS, start Docker Desktop and wait for it to finish starting if the Server section cannot connect.

## Get the project

```bash
git clone https://github.com/DhruvKinger/DockerDemo.git
cd DockerDemo
```

## Run locally

From the project root, run:

```bash
dotnet run --project DockerDemo.csproj --launch-profile http
```

Open [http://localhost:5207](http://localhost:5207). The `http` launch profile is defined in `Properties/launchSettings.json` and sets `ASPNETCORE_ENVIRONMENT=Development`. The app's displayed environment is a separate setting: `APP_ENVIRONMENT`. Its default value is `Local`, configured in `appsettings.json`.

To stop the local app, press `Ctrl+C` in the terminal where it is running.

## Build and run with Docker

Run these commands from the repository root, the directory containing `Dockerfile` and `DockerDemo.csproj`.

### Build the image

```bash
docker build -t docker-demo:latest -f Dockerfile .
```

The final `.` is the Docker build context: it makes the files in the current project directory available to the Dockerfile. The Dockerfile uses the .NET 10 SDK image to publish the app, then copies the published output into the ASP.NET Core 10 runtime image.

### List images

```bash
docker images
```

Look for `docker-demo` with the `latest` tag.

### Start a container

```bash
docker run --name docker-demo-container -d -p 8080:8080 docker-demo:latest
```

The port mapping forwards host port `8080` to container port `8080`. The Dockerfile configures ASP.NET Core to listen on port 8080 and sets `APP_ENVIRONMENT=Docker`.

Open [http://localhost:8080](http://localhost:8080). The page should display the Docker environment. No application-code change is needed for this Docker demo.

### Inspect the running container

```bash
docker ps
docker logs docker-demo-container
```

The logs should include a line stating that the app is listening on port 8080 and that the application started. ASP.NET Core may also log warnings about Data Protection keys not persisting when the container is removed, and about HTTPS redirection when no HTTPS endpoint is configured. For this HTTP-only demo, those warnings do not mean the app failed to start.

### Stop and restart

```bash
docker stop docker-demo-container
docker start docker-demo-container
```

### Remove the demo resources

```bash
docker rm -f docker-demo-container
docker rmi docker-demo:latest
```

## Configuration notes

- `APP_ENVIRONMENT` is a custom application configuration key read by `HomeController`. It is displayed by the home page.
- `appsettings.json` sets `APP_ENVIRONMENT` to `Local` by default.
- The Dockerfile sets `APP_ENVIRONMENT` to `Docker`. Configuration supplied through the container environment takes precedence over the value in `appsettings.json`.
- `ASPNETCORE_ENVIRONMENT` is a different setting that selects the ASP.NET Core hosting environment, such as `Development` or `Production`. It does not set the app's displayed `APP_ENVIRONMENT` value.
- The container serves HTTP on port 8080. Browse to `http://localhost:8080`, not HTTPS.
- The project targets `net10.0`; the generated application assembly is `DockerDemo.dll`.

## Troubleshooting

### Docker cannot connect to the daemon

If Docker reports that it cannot connect to the Docker daemon, start Docker Desktop (macOS/Windows) or the Docker service (Linux), wait until it is ready, and verify with `docker version`. Then retry the build.

### The browser cannot reach the container

Check that the container is running with `docker ps`. Confirm the published ports show host port `8080` mapped to container port `8080`, and open `http://localhost:8080`.

### The image build cannot find the project file

Run `docker build` from the repository root so the final `.` build-context argument includes `DockerDemo.csproj` and the root-level `Dockerfile`.

### The Docker container name is already in use

Remove the earlier container before creating another one:

```bash
docker rm -f docker-demo-container
```

## Project structure

```text
DockerDemo/
├── Controllers/       # MVC controller
├── Models/            # View models
├── Properties/        # Local launch profile
├── Views/             # Razor views
├── wwwroot/            # Static assets
├── DockerDemo.csproj   # .NET 10 web project
├── Dockerfile          # Multi-stage Docker build
├── Program.cs          # ASP.NET Core application entry point
└── appsettings.json    # Default application configuration
```
