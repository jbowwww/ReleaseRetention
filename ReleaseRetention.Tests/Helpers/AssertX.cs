using Data.Entities;
using Environment = Data.Entities.Environment;

namespace ReleaseRetention.Tests.Helpers;

public class AssertX
{
    public static void ProjectEquivalent(Project p1, Project p2)
    {
        Assert.Equal(p1.Id, p2.Id);
        Assert.Equal(p1.Name, p2.Name);
    }
    
    public static void EnvironmentEquivalent(Environment e1, Environment e2)
    {
        Assert.Equal(e1.Id, e2.Id);
        Assert.Equal(e1.Name, e2.Name);
    }

    public static void ReleaseEquivalent(Release r1, Release r2)
    {
        Assert.Equal(r1.Id, r2.Id);
        Assert.Equal(r1.ProjectId, r2.ProjectId);
        Assert.Equal(r1.Version, r2.Version);
        Assert.Equal(r1.Created, r2.Created);
    }

    public static void DeploymentEquivalent(Deployment d1, Deployment d2)
    {
        Assert.Equal(d1.Id, d2.Id);
        Assert.Equal(d1.EnvironmentId, d2.EnvironmentId);
        Assert.Equal(d1.ReleaseId, d2.ReleaseId);
        Assert.Equal(d1.DeployedAt, d2.DeployedAt);
    }
}