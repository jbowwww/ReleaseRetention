using Data.Queries;
using Data.Entities;
using Environment = Data.Entities.Environment;
using Data.Context;
using ReleaseRetention.Tests.Helpers;

namespace ReleaseRetention.Tests.Queries;

using PER = (Project Project, Environment Environment, Release Release, IEnumerable<Deployment> Deployments);

public class QueryTests
{
    [Theory]
    [Trait("Category", "Query")]
    [Trait("Type", nameof(DataContext))]
    [Trait("Category", "Execution")]
    [MemberData(nameof(QueryTests_Data.TestCase00), parameters: 1, MemberType = typeof(QueryTests_Data))]
    [MemberData(nameof(QueryTests_Data.TestCase01), parameters: 1, MemberType = typeof(QueryTests_Data))]
    public void ReleaseHistoryQuery_Execution_ReturnsResults(
        IDataContext dataContext,
        int retainReleaseCount,
        IEnumerable<IGrouping<Release, PER>> expected)
    {
        var query = new RetainReleaseQuery() { RetainReleaseCount = retainReleaseCount };
        var actual = dataContext.Execute(query);

        Assert.Equal(expected.Count(), actual.Count());

        for (var i = 0; i < actual.Count(); i++)
        {
            var actualResult = actual.Skip(i).First();
            var expectedResult = expected.Skip(i).First();
            
            AssertX.ReleaseEquivalent(expectedResult.Key, actualResult.Key);
            Assert.Equal(expectedResult.Count(), actualResult.Count());

            for (var j = 0; j < actualResult?.Count(); j++)
            {
                var actualPERD = actualResult.Skip(j).First();
                var expectedPERD = expectedResult.Skip(j).First();

                AssertX.ProjectEquivalent(expectedPERD.Project, actualPERD.Project);
                AssertX.EnvironmentEquivalent(expectedPERD.Environment, actualPERD.Environment);
                AssertX.ReleaseEquivalent(expectedPERD.Release, actualPERD.Release);
                
                Assert.Equal(expectedPERD.Deployments.Count(), actualPERD.Deployments.Count());

                for (var k = 0; k < actualPERD.Deployments.Count(); k++)
                {
                    var actualDeployment = actualPERD.Deployments.Skip(k).FirstOrDefault();
                    var expectedDeployment = expectedPERD.Deployments.Skip(k).FirstOrDefault();

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
                1,  // ReleaseHistoryQuery.RetainReleaseCount
                // Expected value
                new [] {
                    new TestDataGrouping<Release, PER>(
                        new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                        [(
                            Project: new Project() { Id = "P1", Name = "Project 1", },
                            Environment: new Environment() { Id = "E1", Name = "Environment 1", },
                            Release: new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                            Deployments: [new Deployment() { Id = "D5", ReleaseId = "R2", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:50:00"), },]
                        )]
                    ),
                    new TestDataGrouping<Release, PER>(
                        new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                        [(
                            Project: new Project() { Id = "P1", Name = "Project 1", },
                            Environment: new Environment() { Id = "E2", Name = "Environment 2", },
                            Release: new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                            Deployments: [new Deployment() { Id = "D2", ReleaseId = "R1", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:10:00"), },]
                        )]
                    ),
                    new TestDataGrouping<Release, PER>(
                        new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                        [
                            (
                                Project: new Project() { Id = "P2", Name = "Project 2", },
                                Environment: new Environment() { Id = "E1", Name = "Environment 1", },
                                Release: new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                                Deployments: [new Deployment() { Id = "D6", ReleaseId = "R3", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-09T12:50:00"), },]
                            ),
                            (
                                Project: new Project() { Id = "P2", Name = "Project 2", },
                                Environment: new Environment() { Id = "E2", Name = "Environment 2", },
                                Release: new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                                Deployments: [new Deployment() { Id = "D4", ReleaseId = "R3", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-09T10:15:00"), },]
                              )
                            ]
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
                1,  // ReleaseHistoryQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, PER>(
                        new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },
                        [(
                            Project: new Project() { Id = "P1", Name = "Project 1", },
                            Environment: new Environment() { Id = "E1", Name = "Environment 1", },
                            Release: new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },
                            Deployments: [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2000-01-01T10:00:00"), },]
                        )]
                    ),
                },
            ],
        ];
}
