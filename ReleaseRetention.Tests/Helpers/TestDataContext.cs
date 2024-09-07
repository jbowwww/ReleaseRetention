using ReleaseRetention.Data.Entities;
using Environment = ReleaseRetention.Data.Entities.Environment;

namespace ReleaseRetention.Tests.Helpers;

internal class TestDataContext : Data.Context.DataContext
{
    internal TestDataContext(
        IEnumerable<Project> projects,
        IEnumerable<Environment> environments,
        IEnumerable<Release> releases,
        IEnumerable<Deployment> deployments
    ) {
        Projects = projects;
        Environments = environments;
        Releases = releases;
        Deployments = deployments;
    }
}