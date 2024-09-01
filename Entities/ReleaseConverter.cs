using System.Text.Json;
using System.Text.Json.Serialization;

public class ReleaseConverter : JsonConverter<Release>
{
    public ProjectReleaseInfo ProjectReleaseInfo { get; }

    public ReleaseConverter(ProjectReleaseInfo projectReleaseInfo)
    {
        ProjectReleaseInfo = projectReleaseInfo;
    }

    public override Release? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var release = JsonSerializer.Deserialize<Release>(ref reader, new JsonSerializerOptions());
        if (release != null)
        {
            release.Project = ProjectReleaseInfo.Projects.FirstOrDefault(p => p.Id == release.ProjectId);
        }
        return release;
    }

    public override void Write(Utf8JsonWriter writer, Release value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}