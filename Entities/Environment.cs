public class Environment
{
    public required string Id { get; init; } = null!;
    public required string Name { get; init; } = null!;
    public override string ToString() => $"{typeof(Environment).Name}: Id={Id} Name={Name}";
}