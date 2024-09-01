public class Release
{
    public required string Id { get; init; } = null!;
    public required string ProjectId { get; init; } = null!;
    public Project? Project { get; internal set; }
    public required string? Version { get; init; } = null;
    public required DateTime Created { get; init; } = default;
    public override string ToString() => $"Release: Id={Id} ProjectId={ProjectId} Project.Name=\"{Project?.Name}\" Version={Version} Created={Created}";
}