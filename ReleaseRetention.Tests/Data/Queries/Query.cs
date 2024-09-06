using Data.Queries;
using Data.Entities;
using Environment = Data.Entities.Environment;
using Data.Context;
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
    public void ReleaseHistoryQuery_Execution_ReturnsResults(
        IDataContext dataContext,
        int retainReleaseCount,
        IEnumerable<ReleaseHistoryQuery.Result> expected)
    {
        var query = new ReleaseHistoryQuery() { RetainReleaseCount = retainReleaseCount };

        var actual = dataContext.Execute(query);

        for (var i = 0; i < actual.Count(); i++)
        {
            var actualResult = actual.Skip(i).First();
            var expectedResult = expected.Skip(i).First();
            
            AssertX.ProjectEquivalent(expectedResult.Project, actualResult.Project);
            AssertX.EnvironmentEquivalent(expectedResult.Environment, actualResult.Environment);
            Assert.Equal(expectedResult.Releases.Count(), actualResult.Releases.Count());
            
            for (var j = 0; j < expectedResult.Releases.Count(); j++)
            {
                var actualRelease = actualResult.Releases.Skip(j).First();
                var expectedRelease = expectedResult.Releases.Skip(j).First();

                AssertX.ReleaseEquivalent(expectedRelease.Key, actualRelease.Key);
                
                for (var k = 0; k < expectedRelease.Count(); k++)
                {
                    var expectedDeployment = expectedRelease.Skip(k).First();
                    var actualDeployment = actualRelease.Skip(k).First();
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
                    new ReleaseHistoryQuery.Result() {
                        Project = new Project() { Id = "P1", Name = "Project 1", },
                        Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                        Releases =
                        [
                            new TestDataGrouping<Release, Deployment>(
                                new Release() { Id = "R2", Version = "1.0.1", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:30:00"), },
                                [new Deployment() { Id = "D5", ReleaseId = "R2", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:50:00"), },]
                            ),
                            // new TestDataGrouping<Release, Deployment>(
                            //     new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                            //     [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:00:00"), },]
                            // ),
                        ],
                    },
                    new ReleaseHistoryQuery.Result() {
                        Project = new Project() { Id = "P1", Name = "Project 1", },
                        Environment = new Environment() { Id = "E2", Name = "Environment 2", },
                        Releases =
                        [
                            new TestDataGrouping<Release, Deployment>(
                                new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                                [new Deployment() { Id = "D2", ReleaseId = "R1", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-08T10:10:00"), },]
                            ),
                        ],
                    },
                    new ReleaseHistoryQuery.Result() {
                        Project = new Project() { Id = "P2", Name = "Project 2", },
                        Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                        Releases =
                        [
                            new TestDataGrouping<Release, Deployment>(
                                new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                                [new Deployment() { Id = "D6", ReleaseId = "R3", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-09T12:50:00"), },]
                            ),
                            // new TestDataGrouping<Release, Deployment>(
                            //     new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2020-03-08T09:00:00"), },
                            //     [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2020-03-08T10:00:00"), },]
                            // ),
                        ],
                    },
                    new ReleaseHistoryQuery.Result() {
                        Project = new Project() { Id = "P2", Name = "Project 2", },
                        Environment = new Environment() { Id = "E2", Name = "Environment 2", },
                        Releases =
                        [
                            new TestDataGrouping<Release, Deployment>(
                                new Release() { Id = "R3", Version = "1.0.0", ProjectId = "P2", Created = DateTime.Parse("2020-03-09T08:30:00"), },
                                [new Deployment() { Id = "D4", ReleaseId = "R3", EnvironmentId = "E2", DeployedAt = DateTime.Parse("2020-03-09T10:15:00"), },]
                            ),
                        ],
                    },
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
                    new ReleaseHistoryQuery.Result()
                    {
                        Project = new Project() { Id = "P1", Name = "Project 1", },
                        Environment = new Environment() { Id = "E1", Name = "Environment 1", },
                        Releases =
                        [
                            new TestDataGrouping<Release, Deployment>
                            (
                                new Release() { Id = "R1", Version = "1.0.0", ProjectId = "P1", Created = DateTime.Parse("2000-01-01T08:00:00"), },
                                [new Deployment() { Id = "D1", ReleaseId = "R1", EnvironmentId = "E1", DeployedAt = DateTime.Parse("2000-01-01T10:00:00"), },]
                            ),
                        ],
                    },
                },
            ],
        ];
}
