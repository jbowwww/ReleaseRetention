using Entities;
using Environment = Entities.Environment;

namespace Aggregates;

public class ReleaseHistory
{
    public required Project Project;
    public required Environment Environment;
    public required IEnumerable<IGrouping<Release, Deployment>> Releases;
    public override string ToString() =>
        $"{typeof(ReleaseHistory).Name}:\n{Project}\n{Environment}\n{Releases.ToString(nameof(Releases))}"
        .Replace("\n", "\n\t");
}