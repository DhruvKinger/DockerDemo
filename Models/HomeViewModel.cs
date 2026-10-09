namespace DockerDemo.Models;

public class HomeViewModel
{
    public string Framework { get; set; } = "ASP.NET Core MVC";
    public string Runtime { get; set; } = ".NET 10";
    public string EnvironmentLabel { get; set; } = "UNKNOWN";
    public string EnvironmentMessage { get; set; } = "Environment configuration is not set.";
    public string ApplicationStatus { get; set; } = "Running";
    public string AppEnvironment { get; set; } = "Unknown";
}
