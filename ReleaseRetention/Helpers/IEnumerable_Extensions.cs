using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseRetention.Helpers;

public static class IEnumerable_Extensions
{
    public static string ToString<T>(this IEnumerable<T>? values) => values.ToString("[Enumerable]");
    public static string ToString<T>(this IEnumerable<T>? values, string name) =>
        ($"{name} ({values?.Count()}):\n" + string.Join("\n", (values ?? [])
            .Select<T, object>(value => value == null ? "(null)" :
                value.GetType().GetInterfaces().Any(i => i.Name.Contains("IGrouping")) ?
                    (value as IGrouping<object, object>).ToString("[IGrouping]") : 
                (value?.ToString() ?? "(null)")
        ))).Replace("\n", "\n\t");

    public static string ToString(this IEnumerable? values) => values.ToString("[Enumerable]");
    public static string ToString(this IEnumerable? values, string name) =>
        ToString(values?.Cast<object>(), name);
}