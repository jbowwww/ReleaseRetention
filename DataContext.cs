using System.Text.Json;

public static class DataContext
{
    public static IEnumerable<Project> Projects { get; private set; } = [];

    public static IEnumerable<Environment> Environments { get; private set; } = [];

    public static IEnumerable<Release> Releases { get; private set; } = [];

    public static IEnumerable<Deployment> Deployments { get; private set; } = [];

    internal static T[] Deserialize<T>(string path, JsonSerializerOptions? options = null) =>
        JsonSerializer.Deserialize<T[]>(File.ReadAllBytes(path), options ?? new JsonSerializerOptions()) ?? [];
    
    public static void Load(string pathPrefix = "./data")
    {
        var options = new JsonSerializerOptions() {
            Converters = {
                new ReleaseConverter(),
                new DeploymentConverter(),
            },
        };
        Projects = Deserialize<Project>(Path.Join(pathPrefix, "./Projects.json"), options).OrderBy(p => p.Id);
        Environments = Deserialize<Environment>(Path.Join(pathPrefix, "./Environments.json"), options).OrderBy(e => e.Id);
        Releases = Deserialize<Release>(Path.Join(pathPrefix, "./Releases.json"), options).OrderByDescending(r => r.Created);
        Deployments = Deserialize<Deployment>(Path.Join(pathPrefix, "./Deployments.json"), options).OrderByDescending(d => d.DeployedAt);
    }

    public static new string ToString() => $"{nameof(DataContext)}:" +
        $"\n\tProjects ({Projects?.Count()}): {string.Join(", ", Projects ?? [])}" +
        $"\n\tEnvironments ({Environments?.Count()}): {string.Join(", ", Environments ?? [])}" +
        $"\n\tReleases ({Releases?.Count()}): {string.Join(", ", Releases ?? [])}" +
        $"\n\tDeployments ({Deployments?.Count()}): {string.Join(", ", Deployments ?? [])}";
}