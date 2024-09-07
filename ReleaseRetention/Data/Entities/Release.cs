using System;

namespace Data.Entities;

public class Release
{
    public required string Id { get; init; } = null!;
    public required string ProjectId { get; init; } = null!;
    public required string? Version { get; init; } = null;
    public required DateTime Created { get; init; } = default;
    public override string ToString() => $"{typeof(Release).Name}: Id={Id} ProjectId={ProjectId} Version={Version} Created={Created}";
}