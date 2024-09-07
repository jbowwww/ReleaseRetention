using ReleaseRetention.Data.Queries;
using ReleaseRetention.Data.Entities;
using Environment = ReleaseRetention.Data.Entities.Environment;
using ReleaseRetention.Data.Context;

using ReleaseRetention.Tests.Helpers;

namespace ReleaseRetention.Tests.Queries;

public class QueryTests
{
    [Theory]
    [Trait("Category", "Query")]
    [Trait("Type", nameof(DataContext))]
    [Trait("Category", "Execution")]
    [MemberData(nameof(QueryTests_Data.TestCase00), parameters: 1, MemberType = typeof(QueryTests_Data))]
    [MemberData(nameof(QueryTests_Data.TestCase01), parameters: 1, MemberType = typeof(QueryTests_Data))]
    public void RetainReleaseQuery_Execution_ReturnsResults(
        IDataContext dataContext,
        int retainReleaseCount,
        IEnumerable<IGrouping<Release, RetainReleaseQuery.Result>> expected)
    {
        var query = new RetainReleaseQuery() { RetainReleaseCount = retainReleaseCount };
        var actual = dataContext.Execute(query);

        Assert.Equal(expected.Count(), actual.Count());

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

public class QueryTests_Data
{
    public static IEnumerable<object[]> TestCase00(int retainReleaseCount) =>
        [
            [
                new TestDataContext
                (
                    [
                        new Project() { Id = "P1", Name = "Project 1", },
                        new Project() { Id = "P2", Name = "Project 2", },
                    ],
                    [
                        new Environment() { Id = "E1", Name = "Environment 1", },
                        new Environment() { Id = "E2", Name = "Environment 2", },
                    ],
                    [
                        new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                        new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                        new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                        new Release() { Id = "R4", Version = "1.0.1", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:45:00"), },
                    ],
                    [
                        new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:00:00"), },
                        new Deployment() { Id = "D2", ReleaseId = "R1", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:10:00"), },
                        new Deployment() { Id = "D3", ReleaseId = "R4", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-09T09:30:00"), },
                        new Deployment() { Id = "D4", ReleaseId = "R3", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-09T10:15:00"), },
                        new Deployment() { Id = "D5", ReleaseId = "R2", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:50:00"), },
                        new Deployment() { Id = "D6", ReleaseId = "R3", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-09T12:50:00"), },
                    ]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new [] {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>(
                        new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                        [new RetainReleaseQuery.Result()
                        {
                            Project = new Project() { Id = "P1", Name = "Project 1", },
                            Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                            Release = new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                            Deployments = [new Deployment() { Id = "D5", ReleaseId = "R2", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:50:00"), },]
                        }]
                    ),
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>(
                        new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                        [new RetainReleaseQuery.Result()
                        {
                            Project = new Project() { Id = "P1", Name = "Project 1", },
                            Environment = new Environment() { Id = "E2", Name = "Environment 2", },
                            Release = new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                            Deployments = [new Deployment() { Id = "D2", ReleaseId = "R1", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:10:00"), },]
                        }]
                    ),
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>(
                        new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                        [new RetainReleaseQuery.Result()
                        {
                            Project = new Project() { Id = "P2", Name = "Project 2", },
                            Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                            Release = new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                            Deployments = [new Deployment() { Id = "D6", ReleaseId = "R3", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-09T12:50:00"), },]
                        },
                        new RetainReleaseQuery.Result()
                        {
                            Project = new Project() { Id = "P2", Name = "Project 2", },
                            Environment = new Environment() { Id = "E2", Name = "Environment 2", },
                            Release = new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                            Deployments = [new Deployment() { Id = "D4", ReleaseId = "R3", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-09T10:15:00"), },]
                        }]
                    )
                }
            ]
        ];

    public static IEnumerable<object[]> TestCase01(int retainReleaseCount) =>
        [
            [
                new TestDataContext
                (
                    [new Project() { Id = "P1", Name = "Project 1", },],
                    [new Environment() { Id = "E1", Name = "Environment 1", },],
                    [new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },],
                    [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2000-01-01T10:00:00"), },]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>(
                        new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },
                        [new RetainReleaseQuery.Result() {
                            Project = new Project() { Id = "P1", Name = "Project 1", },
                            Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                            Release = new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },
                            Deployments = [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2000-01-01T10:00:00"), },]
                        }]
                    ),
                },
            ],
        ];
}
