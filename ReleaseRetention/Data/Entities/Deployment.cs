using System;

namespace Data.Entities;

public class Deployment
{
    public required string Id { get; init; } = null!;
    public required string ReleaseId { get; init; } = null!;
    public required string EnvironmentId { get; init; } = null!;
    public required DateTime DeployedAt { get; init; } = default;
    public override string ToString() => $"{typeof(Deployment).Name}: Id=\"{Id}\" ReleaseId=\"{ReleaseId}\" EnvironmentId=\"{EnvironmentId}\" DeployedAt=\"{DeployedAt}\"";
}