using System.Text.Json.Serialization;

public class ProjectReleaseInfo
{
    public string PathPrefix { get; } = string.Empty;

    public IEnumerable<Project> Projects { get; }

    public IEnumerable<Environment> Environments { get; }

    public IEnumerable<Release> Releases { get; }

    public IEnumerable<Deployment> Deployments { get; }

    public ProjectReleaseInfo(string pathPrefix = "./data")
    {
        PathPrefix = pathPrefix;

        var converters = new JsonConverter[] {
            new ReleaseConverter(this),
            new DeploymentConverter(this),
        };
        
        Projects = JsonData.Read<Project>(Path.Join(PathPrefix, "./Projects.json"));
        Environments = JsonData.Read<Environment>(Path.Join(PathPrefix, "./Environments.json"));
        Releases = JsonData.Read<Release>(Path.Join(PathPrefix, "./Releases.json"), converters);
        Deployments = JsonData.Read<Deployment>(Path.Join(PathPrefix, "./Deployments.json"), converters);
    }

    public override string ToString() => $"{base.ToString()}:" +
        $"\n\tProjects ({Projects?.Count()}): {string.Join(", ", Projects ?? [])}" +
        $"\n\tEnvironments ({Environments?.Count()}): {string.Join(", ", Environments ?? [])}" +
        $"\n\tReleases ({Releases?.Count()}): {string.Join(", ", Releases ?? [])}" +
        $"\n\tDeployments ({Deployments?.Count()}): {string.Join(", ", Deployments ?? [])}";
}