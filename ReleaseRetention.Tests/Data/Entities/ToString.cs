using Data.Entities;
using Environment = Data.Entities.Environment;

namespace ReleaseRetention.Tests.Entities;

public class ToStringTests
{
    [Fact]
    [Trait("Category", "Entity")]
    [Trait("Type", nameof(Project))]
    [Trait("Category", "ToString")]
    public void Project_ToString_FormatsInstanceValuesCorrectly()
    {
        var entity = new Project()
        {
            Id = "Id-1",
            Name = "Name-1",
        };
        var actual = entity.ToString();
        var expected = $"{nameof(Project)}: Id={entity.Id} Name={entity.Name}";
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    [Trait("Category", "Entity")]
    [Trait("Type", nameof(Environment))]
    [Trait("Category", "ToString")]
    public void Environment_ToString_FormatsInstanceValuesCorrectly()
    {
        var entity = new Environment()
        {
            Id = "Id-1",
            Name = "Name-1",
        };
        var actual = entity.ToString();
        var expected = $"{nameof(Environment)}: Id={entity.Id} Name={entity.Name}";
        Assert.Equal(expected, actual);
    }

    [Fact]
    [Trait("Category", "Entity")]
    [Trait("Type", nameof(Release))]
    [Trait("Category", "ToString")]
    public void Release_ToString_FormatsInstanceValuesCorrectly()
    {
        var entity = new Release()
        {
            Id = "Id-1",
            ProjectId = "Project-Id-1",
            Version = "1.0.1",
            Created = new DateTime(1996, 12, 25, 11, 08, 01),
        };
        var actual = entity.ToString();
        var expected = $"{typeof(Release).Name}: Id={entity.Id} ProjectId={entity.ProjectId} Version={entity.Version} Created={entity.Created}";
        Assert.Equal(expected, actual);
    }

    [Fact]
    [Trait("Category", "Entity")]
    [Trait("Type", nameof(Deployment))]
    [Trait("Category", "ToString")]
    public void Deployment_ToString_FormatsInstanceValuesCorrectl()
    {
        var entity = new Deployment()
        {
            Id = "Id-1",
            ReleaseId = "Release-Id-1",
            EnvironmentId = "Environment-Id-1",
            DeployedAt = new DateTime(1998, 4, 19, 03, 48, 08),
        };
        var actual = entity.ToString();
        var expected = $"{typeof(Deployment).Name}: Id=\"{entity.Id}\" ReleaseId=\"{entity.ReleaseId}\" EnvironmentId=\"{entity.EnvironmentId}\" DeployedAt=\"{entity.DeployedAt}\"";
        Assert.Equal(expected, actual);
    }
}