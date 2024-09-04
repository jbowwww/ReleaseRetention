using Data.Entities;
using Environment = Data.Entities.Environment;

namespace ReleaseRetention.Tests.Entities;

public class ConstructionTests
{
    [Fact]
    [Trait("Category", "Entity")]
    [Trait("EntityType", nameof(Project))]
    [Trait("Category", "Construction")]
    public void Project_Construction_SetsPropertiesAndReturnsInstance()
    {
        var expected = new
        {
            Id = "Id-1",
            Name = "Name-1"
        };
        var actual = new Project()
        {
            Id = expected.Id,
            Name = expected.Name
        };
        Assert.IsType<Project>(actual);
        Assert.Equivalent(expected, actual, false);
    }
    
    [Fact]
    [Trait("Category", "Entity")]
    [Trait("EntityType", nameof(Environment))]
    [Trait("Category", "Construction")]
    public void Environment_Construction_SetsPropertiesAndReturnsInstance()
    {
        var expected = new
        {
            Id = "Id-1",
            Name = "Name-1"
        };
        var actual = new Environment()
        {
            Id = expected.Id,
            Name = expected.Name
        };
        Assert.IsType<Environment>(actual);
        Assert.Equivalent(expected, actual, true);
    }

    [Fact]
    [Trait("Category", "Entity")]
    [Trait("EntityType", nameof(Release))]
    [Trait("Category", "Construction")]
    public void Release_Construction_SetsPropertiesAndReturnsInstance()
    {
        var expected = new
        {
            Id = "Id-1",
            ProjectId = "Project-Id-1",
            Version = "1.0.1",
            Created = new DateTime(1996, 12, 25, 11, 08, 01),
        };
        var actual = new Release()
        {
            Id = "Id-1",
            ProjectId = "Project-Id-1",
            Version = "1.0.1",
            Created = new DateTime(1996, 12, 25, 11, 08, 01),
        };
        Assert.IsType<Release>(actual);
        Assert.Equivalent(expected, actual, false);
    }

    [Fact]
    [Trait("Category", "Entity")]
    [Trait("EntityType", nameof(Deployment))]
    [Trait("Category", "Construction")]
    public void Deployment_Construction_SetsPropertiesAndReturnsInstance()
    {
        var expected = new
        {
            Id = "Id-1",
            ReleaseId = "Release-Id-1",
            EnvironmentId = "Environment-Id-1",
            DeployedAt = new DateTime(1998, 4, 19, 03, 48, 08),
        };
        var actual = new Deployment()
        {
            Id = "Id-1",
            ReleaseId = "Release-Id-1",
            EnvironmentId = "Environment-Id-1",
            DeployedAt = new DateTime(1998, 4, 19, 03, 48, 08),
        };
        Assert.IsType<Deployment>(actual);
        Assert.Equivalent(expected, actual, false);
    }
}