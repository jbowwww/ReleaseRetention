using System.Collections.Generic;
using System.Linq;

public static class IGrouping_Extensions
{
    public static string ToString<TKey, TElement>(this IGrouping<TKey, TElement>? group, string name) =>
        ($"{name} key=\"{(group != null ? group.Key : "(key:null)")}\" ({group?.Count()}):\n" +
        string.Join("\n", (group as IEnumerable<TElement> ?? []).Select(value => value == null ? "(null)" :
            value.GetType().IsAssignableTo(typeof(IEnumerable<TElement>)) ?
                $"Group: Key={group?.Key?.ToString() ?? "(null)"} {(value as IEnumerable<TElement>)!.ToString("Element")}" :
                ("[def]: " + (value?.ToString() ?? "(null)")
        )))).Replace("\n", "\n\t");
}