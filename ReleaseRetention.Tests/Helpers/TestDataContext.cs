namespace ReleaseRetention.Tests.Helpers;

internal class TestDataContext : Data.Context.DataContext
{
    internal TestDataContext(
        IEnumerable<Data.Entities.Project> projects,
        IEnumerable<Data.Entities.Environment> environments,
        IEnumerable<Data.Entities.Release> releases,
        IEnumerable<Data.Entities.Deployment> deployments
    ) {
        Projects = projects;
        Environments = environments;
        Releases = releases;
        Deployments = deployments;
    }
}