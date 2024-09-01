using System.Text.Json;
using System.Text.Json.Serialization;

public class DeploymentConverter : JsonConverter<Deployment>
{
    public ProjectReleaseInfo ProjectReleaseInfo { get; }

    public DeploymentConverter(ProjectReleaseInfo projectReleaseInfo)
    {
        ProjectReleaseInfo = projectReleaseInfo;
    }

    public override Deployment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Console.WriteLine($"Releases={string.Join(", ", ProjectReleaseInfo.Releases)}");
        Console.WriteLine($"Environments={string.Join(", ", ProjectReleaseInfo.Environments)}");
        var deployment = JsonSerializer.Deserialize<Deployment>(ref reader, new JsonSerializerOptions());
        Console.WriteLine($"deployment={deployment}");
        if (deployment != null)
        {
            deployment.Release = ProjectReleaseInfo.Releases.FirstOrDefault(r => r.Id == deployment.ReleaseId);
            deployment.Environment = ProjectReleaseInfo.Environments.FirstOrDefault(e => e.Id == deployment.EnvironmentId);
        }
        return deployment;
    }

    public override void Write(Utf8JsonWriter writer, Deployment value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}