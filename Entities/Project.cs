public class Project
{
    public required string Id { get; init; } = null!;
    public required string Name { get; init; } = null!;

    // Releases associated with this Project
    public IEnumerable<Release> Releases =>
        DataContext.Releases
        .Where(r => r.ProjectId == Id)
        .OrderByDescending(r => r.Created);

    public override string ToString() => $"Project: Id={Id} Name={Name}";
}