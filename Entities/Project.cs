public class Project
{
    public required string Id { get; init; } = null!;
    public required string Name { get; init; } = null!;
    public override string ToString() => $"Project: Id={Id} Name={Name}";
}