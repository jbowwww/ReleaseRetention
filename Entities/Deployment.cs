public class Deployment
{
    public required string Id { get; init; } = null!;
    public required string ReleaseId { get; init; } = null!;
    public Release? Release { get; set; }
    public required string EnvironmentId { get; init; } = null!;
    public Environment? Environment { get; set; }
    public required DateTime DeployedAt { get; init; } = default;
    public override string ToString() => $"Deployment: Id=\"{Id}\" ReleaseId=\"{ReleaseId}\" Release.Version=\"{Release?.Version ?? "(null)"}\" EnvironmentId=\"{EnvironmentId}\" Environment.Name=\"{Environment?.Name ?? "(null)"}\" DeployedAt=\"{DeployedAt}\"";
}