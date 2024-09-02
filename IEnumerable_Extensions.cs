public static class IEnumerable_Extensions
{
    public static string ToString<T>(this IEnumerable<T> strings, string name = "[Enumerable]") =>
        $"\n\n{name} ({strings?.Count()}):\n\t{string.Join("\n\t", strings ?? [])}";
}