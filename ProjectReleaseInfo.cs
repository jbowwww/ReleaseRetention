using System.Text.Json.Serialization;

public class ProjectReleaseInfo
{
    public string PathPrefix { get; } = string.Empty;

    public IList<Project> Projects { get; init; } = [];

    public IList<Environment> Environments { get; init; } = [];

    public IList<Release> Releases { get; init; } = [];

    public IList<Deployment> Deployments { get; init; } = [];

    public ProjectReleaseInfo()
    {
        Projects.AddRange(JsonData.Read<Project>(Path.Join(PathPrefix, "./data/Projects.json")));
        Environments.AddRange(JsonData.Read<Environment>(Path.Join(PathPrefix, "./data/Environments.json")));
        var converters = new JsonConverter[] {
            new ReleaseConverter(this),
            new DeploymentConverter(this),
        };
        Releases.AddRange(JsonData.Read<Release>(Path.Join(PathPrefix, "./data/Releases.json"), converters));//new ReleaseConverter(this)));
        Deployments.AddRange(JsonData.Read<Deployment>(Path.Join(PathPrefix, "./data/Deployments.json"), converters));//, new DeploymentConverter(this)));
    }

    public override string ToString() => $"{base.ToString()}:" +
            $"\n\tProjects ({Projects?.Count()}): {string.Join(", ", Projects ?? [])}" +
            $"\n\tEnvironments ({Environments?.Count()}): {string.Join(", ", Environments ?? [])}" +
            $"\n\tReleases ({Releases?.Count()}): {string.Join(", ", Releases ?? [])}" +
            $"\n\tDeployments ({Deployments?.Count()}): {string.Join(", ", Deployments ?? [])}";
}