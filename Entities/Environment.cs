public class Environment
{
    public required string Id { get; init; } = null!;
    public required string Name { get; init; } = null!;
    public override string ToString() => $"Environment: Id={Id} Name={Name}";
}