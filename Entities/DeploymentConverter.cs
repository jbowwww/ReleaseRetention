using System.Text.Json;
using System.Text.Json.Serialization;

public class DeploymentConverter : JsonConverter<Deployment>
{
    public override Deployment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var deployment = JsonSerializer.Deserialize<Deployment>(ref reader, new JsonSerializerOptions());
        if (deployment != null)
        {
            deployment.Release = DataContext.Releases.FirstOrDefault(r => r.Id == deployment.ReleaseId);
            deployment.Environment = DataContext.Environments.FirstOrDefault(e => e.Id == deployment.EnvironmentId);
        }
        return deployment;
    }

    public override void Write(Utf8JsonWriter writer, Deployment value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}