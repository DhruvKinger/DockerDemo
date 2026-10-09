using System.Diagnostics;
using DockerDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DockerDemo.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var configuredValue = _configuration["APP_ENVIRONMENT"];
        var normalized = configuredValue?.Trim();

        string environmentLabel;
        string environmentMessage;

        if (string.Equals(normalized, "Local", StringComparison.OrdinalIgnoreCase))
        {
            environmentLabel = "LOCAL";
            environmentMessage = "Running directly on your development machine.";
        }
        else if (string.Equals(normalized, "Docker", StringComparison.OrdinalIgnoreCase))
        {
            environmentLabel = "DOCKER";
            environmentMessage = "Running inside a Docker container.";
        }
        else if (string.IsNullOrWhiteSpace(normalized))
        {
            environmentLabel = "UNKNOWN";
            environmentMessage = "APP_ENVIRONMENT is not configured. Set it to Local or Docker to change the behavior.";
        }
        else
        {
            environmentLabel = normalized.ToUpperInvariant();
            environmentMessage = $"Custom environment configured as {normalized}.";
        }

        var model = new HomeViewModel
        {
            AppEnvironment = normalized ?? "Unknown",
            EnvironmentLabel = environmentLabel,
            EnvironmentMessage = environmentMessage
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
