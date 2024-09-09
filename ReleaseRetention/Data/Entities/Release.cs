using System;

namespace ReleaseRetention.Data.Entities;

public record Release(string Id, string ProjectId, string? Version, DateTime Created);