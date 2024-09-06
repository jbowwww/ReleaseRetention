using Data.Entities;
using ReleaseRetention.Tests.Helpers;
using Environment = Data.Entities.Environment;

namespace ReleaseRetention.Tests.DataContext;

public class TestDataContextTests
{
    [Fact]
    [Trait("Category", "DataContext")]
    [Trait("Type", nameof(DataContext))]
    [Trait("Category", "Construction")]
    public void TestDataContext_Construction_LoadsData()
    {
        var expected = new {
            Projects = new []
            {
                new Project() { Id = "P1", Name = "Project 1", },
                new Project() { Id = "P2", Name = "Project 2", },
            },
            Environments = new []
            {
                new Environment() { Id = "E1", Name = "Environment 1", },
                new Environment() { Id = "E2", Name = "Environment 2", },
            },
            Releases = new []
            {
                new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-08T08:30:00"), },
                new Release() { Id = "R4", Version = "1.0.1", ProjectId = "P2", Created = DateTime.Parse("2020-03-08T08:45:00"), },
            },
            Deployments = new []
            {
                new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:00:00"), },
                new Deployment() { Id = "D2", ReleaseId = "R1", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:10:00"), },
                new Deployment() { Id = "D3", ReleaseId = "R4", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T09:30:00"), },
                new Deployment() { Id = "D4", ReleaseId = "R3", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:15:00"), },
                new Deployment() { Id = "D1", ReleaseId = "R3", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:50:00"), },
            },
        };

        var myDataContext = new TestDataContext(expected.Projects, expected.Environments, expected.Releases, expected.Deployments);
        
        Assert.IsType<TestDataContext>(myDataContext);
        Assert.Distinct(myDataContext.Projects);
        Assert.Equal(myDataContext.Projects.Count(), expected.Projects.Length);
        Assert.All(myDataContext.Projects, p => Assert.IsType<Project>(p));
    }
}