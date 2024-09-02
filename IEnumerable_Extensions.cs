using System.Collections;

public static class IEnumerable_Extensions
{
    public static string ToString<T>(this IEnumerable<T>? values, string name = "[Enumerable]") =>
        $"{name} ({values?.Count()}):\n\t{string.Join("\n\t", (values ?? [])
            .Select<T, object>(value => value != null && value.GetType().IsAssignableFrom(typeof(IEnumerable)) ?
                ToString(value as IEnumerable) : value!))}";

    public static string ToString(this IEnumerable? values, string name = "[Enumerable]") =>
        ToString(values?.Cast<object>() ?? [], name);
}