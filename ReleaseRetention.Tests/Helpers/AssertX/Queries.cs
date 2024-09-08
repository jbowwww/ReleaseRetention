using ReleaseRetention.Data.Entities;
using ReleaseRetention.Data.Queries;

namespace ReleaseRetention.Tests.Helpers;

public partial class AssertX
{
    public static void RetainReleaseQueryResultEquivalent
    (
        IEnumerable<IGrouping<Release, RetainReleaseQuery.Result>> expected,
        IEnumerable<IGrouping<Release, RetainReleaseQuery.Result>> actual
    ) {
        for (var i = 0; i < actual.Count(); i++)
        {
            var actualRelease = actual.Skip(i).First();
            var expectedRelease = expected.Skip(i).First();

            AssertX.ReleaseEquivalent(expectedRelease.Key, actualRelease.Key);
            Assert.Equal(expectedRelease.Count(), actualRelease.Count());

            for (var j = 0; j < actualRelease?.Count(); j++)
            {
                var actualReleaseDetails = actualRelease.Skip(j).First();
                var expectedReleaseDetails = expectedRelease.Skip(j).First();

                AssertX.ProjectEquivalent(expectedReleaseDetails.Project, actualReleaseDetails.Project);
                AssertX.EnvironmentEquivalent(expectedReleaseDetails.Environment, actualReleaseDetails.Environment);
                AssertX.ReleaseEquivalent(expectedReleaseDetails.Release, actualReleaseDetails.Release);

                Assert.Equal(expectedReleaseDetails.Deployments.Count(), actualReleaseDetails.Deployments.Count());

                for (var k = 0; k < actualReleaseDetails.Deployments.Count(); k++)
                {
                    var actualDeployment = actualReleaseDetails.Deployments.Skip(k).FirstOrDefault();
                    var expectedDeployment = expectedReleaseDetails.Deployments.Skip(k).FirstOrDefault();

                    AssertX.DeploymentEquivalent(expectedDeployment, actualDeployment);
                }
            }
        }
    }
}