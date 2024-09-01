public static class IList_Extensions
{
    public static int AddRange<T>(this IList<T> target, IEnumerable<T> values) {
        foreach (var value in values)
        {
            target.Add(value);
        }
        return values.Count();
    }
}