using System;

namespace ReleaseRetention.Data.Entities;

public record Deployment(string Id, string ReleaseId, string EnvironmentId, DateTime DeployedAt);