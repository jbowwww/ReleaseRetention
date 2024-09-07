using System.IO;
using System.Linq;
using System.Text.Json;
using ReleaseRetention.Data.Entities;
using Environment = ReleaseRetention.Data.Entities.Environment;

namespace ReleaseRetention.Data.Context;

public class JsonFilesDataContext : DataContext
{
    public string PathPrefix { get; }

    public JsonFilesDataContext(string pathPrefix = "./data")
    {
        PathPrefix = pathPrefix;
        Projects = Deserialize<Project>("Projects.json").OrderBy(p => p.Id);
        Environments = Deserialize<Environment>("Environments.json").OrderBy(e => e.Id);
        Releases = Deserialize<Release>("Releases.json").OrderByDescending(r => r.Created);
        Deployments = Deserialize<Deployment>("Deployments.json").OrderByDescending(d => d.DeployedAt);
    }
    
    private T[] Deserialize<T>(string filename, JsonSerializerOptions? options = null) =>
        JsonSerializer.Deserialize<T[]>(
            File.ReadAllBytes(Path.Join(PathPrefix, filename)),
            options ?? new JsonSerializerOptions()
        ) ?? [];

    public override string ToString() => $"[{nameof(JsonFilesDataContext)}]:" +
        $"\n\tProjects ({Projects?.Count()}): {string.Join(", ", Projects ?? [])}" +
        $"\n\tEnvironments ({Environments?.Count()}): {string.Join(", ", Environments ?? [])}" +
        $"\n\tReleases ({Releases?.Count()}): {string.Join(", ", Releases ?? [])}" +
        $"\n\tDeployments ({Deployments?.Count()}): {string.Join(", ", Deployments ?? [])}";
}