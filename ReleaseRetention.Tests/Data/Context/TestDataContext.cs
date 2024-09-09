using ReleaseRetention.Data.Entities;
using Environment = ReleaseRetention.Data.Entities.Environment;
using ReleaseRetention.Tests.Helpers;

namespace ReleaseRetention.Tests.DataContext;

public class TestDataContextTests
{
    [Fact]
    [Trait("Category", "Context")]
    [Trait("Type", nameof(TestDataContext))]
    [Trait("Category", "Construction")]
    public void TestDataContext_Construction_LoadsData()
    {
        var expected = new {
            Projects = new []
            {
                new Project("P1", "Project 1"),
                new Project("P2", "Project 2"),
            },
            Environments = new []
            {
                new Environment("E1", "Environment 1"),
                new Environment("E2", "Environment 2"),
            },
            Releases = new []
            {
                new Release("R1", "1.0.0", "P1", DateTime.Parse("2020-03-08T09:00:00")),
                new Release("R2", "1.0.1", "P1", DateTime.Parse("2020-03-08T09:30:00")),
                new Release("R3", "1.0.0", "P2", DateTime.Parse("2020-03-08T08:30:00")),
                new Release("R4", "1.0.1", "P2", DateTime.Parse("2020-03-08T08:45:00")),
            },
            Deployments = new []
            {
                new Deployment("D1", "R1", "E1", DateTime.Parse("2020-03-08T10:00:00")),
                new Deployment("D2", "R1", "E2", DateTime.Parse("2020-03-08T10:10:00")),
                new Deployment("D3", "R4", "E2", DateTime.Parse("2020-03-08T09:30:00")),
                new Deployment("D4", "R3", "E2", DateTime.Parse("2020-03-08T10:15:00")),
                new Deployment("D1", "R3", "E1", DateTime.Parse("2020-03-08T10:50:00")),
            },
        };

        var myDataContext = new TestDataContext(expected.Projects, expected.Environments, expected.Releases, expected.Deployments);
        
        Assert.IsType<TestDataContext>(myDataContext);
        Assert.Distinct(myDataContext.Projects);
        Assert.Equal(myDataContext.Projects.Count(), expected.Projects.Length);
        Assert.All(myDataContext.Projects, p => Assert.IsType<Project>(p));
    }
}