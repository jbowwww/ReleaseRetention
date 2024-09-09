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
    [Trait("Type", nameof(RetainReleaseQuery))]
    [Trait("Category", "Execution")]
    [MemberData(nameof(QueryTests_Data.TestCase00), parameters: 1, MemberType = typeof(QueryTests_Data))]
    [MemberData(nameof(QueryTests_Data.TestCase01), parameters: 1, MemberType = typeof(QueryTests_Data))]
    [MemberData(nameof(QueryTests_Data.TestCase02), parameters: 1, MemberType = typeof(QueryTests_Data))]
    [MemberData(nameof(QueryTests_Data.TestCase03), parameters: 1, MemberType = typeof(QueryTests_Data))]
    public void RetainReleaseQuery_Execution_ReturnsResults(
        IDataContext dataContext,
        int retainReleaseCount,
        IEnumerable<IGrouping<Release, RetainReleaseQuery.Result>> expected)
    {
        var query = new RetainReleaseQuery() { RetainReleaseCount = retainReleaseCount };
        
        var actual = dataContext.Execute(query);

        AssertX.RetainReleaseQueryResultEquivalent(expected, actual);
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
                        new Project("P1", "Project 1"),
                        new Project("P2", "Project 2"),
                    ],
                    [
                        new Environment("E1", "Environment 1"),
                        new Environment("E2", "Environment 2"),
                    ],
                    [
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2020-03-08T09:00:00")),
                        new Release("R2", "1.0.1", "P1", DateTime.Parse("2020-03-08T09:30:00")),
                        new Release("R3", "1.0.0", "P2", DateTime.Parse("2020-03-09T08:30:00")),
                        new Release("R4", "1.0.1", "P2", DateTime.Parse("2020-03-09T08:45:00")),
                    ],
                    [
                        new Deployment("D1", "R1", "E1", DateTime.Parse("2020-03-08T10:00:00")),
                        new Deployment("D2", "R1", "E2", DateTime.Parse("2020-03-08T10:10:00")),
                        new Deployment("D3", "R4", "E2", DateTime.Parse("2020-03-09T09:30:00")),
                        new Deployment("D4", "R3", "E2", DateTime.Parse("2020-03-09T10:15:00")),
                        new Deployment("D5", "R2", "E1", DateTime.Parse("2020-03-08T10:50:00")),
                        new Deployment("D6", "R3", "E1", DateTime.Parse("2020-03-09T12:50:00"))
                    ]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2020-03-08T09:00:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E2", "Environment 2"),
                                new Release("R1", "1.0.0", "P1", DateTime.Parse("2020-03-08T09:00:00")),
                                [new Deployment("D2", "R1", "E2", DateTime.Parse("2020-03-08T10:10:00")),]
                            )
                        ]
                    ),
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R2", "1.0.1", "P1", DateTime.Parse("2020-03-08T09:30:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E1", "Environment 1"),
                                new Release("R2", "1.0.1", "P1", DateTime.Parse("2020-03-08T09:30:00")),
                                [new Deployment("D5", "R2", "E1", DateTime.Parse("2020-03-08T10:50:00")),]
                            )
                        ]
                    ),
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R3", "1.0.0", "P2", DateTime.Parse("2020-03-09T08:30:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P2", "Project 2"),
                                new Environment("E1", "Environment 1"),
                                new Release("R3", "1.0.0", "P2", DateTime.Parse("2020-03-09T08:30:00")),
                                [new Deployment("D6", "R3", "E1", DateTime.Parse("2020-03-09T12:50:00")),]
                            ),
                            new RetainReleaseQuery.Result
                            (
                                new Project("P2", "Project 2"),
                                new Environment("E2", "Environment 2"),
                                new Release("R3", "1.0.0", "P2", DateTime.Parse("2020-03-09T08:30:00")),
                                [new Deployment("D4", "R3", "E2", DateTime.Parse("2020-03-09T10:15:00")),]
                            )
                        ]
                    )
                }
            ]
        ];

    [Trait("Description", "1 Release, Keep 1")]
    public static IEnumerable<object[]> TestCase01(int retainReleaseCount) =>
        [
            [
                new TestDataContext
                (
                    [new Project("P1", "Project 1"),],
                    [new Environment("E1", "Environment 1"),],
                    [new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),],
                    [new Deployment("D1", "R1", "E1", DateTime.Parse("2000-01-01T10:00:00")),]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E1", "Environment 1"),
                                new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                                [new Deployment("D1", "R1", "E1", DateTime.Parse("2000-01-01T10:00:00")),]
                            )
                        ]
                    ),
                },
            ],
        ];
    
    [Trait("Description", "2 Releases, 1 Environment, Keep 1")]
    public static IEnumerable<object[]> TestCase02(int retainReleaseCount) =>
        [
            [
                new TestDataContext
                (
                    [new Project("P1", "Project 1"),],
                    [new Environment("E1", "Environment 1"),],
                    [
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                        new Release("R2", "1.0.1", "P1", DateTime.Parse("2000-01-01T09:00:00")),
                    ],
                    [
                        new Deployment("D1", "R2", "E1", DateTime.Parse("2000-01-01T10:00:00")),
                        new Deployment("D2", "R1", "E1", DateTime.Parse("2000-01-01T11:00:00")),
                    ]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E1", "Environment 1"),
                                new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                                [new Deployment("D2", "R1", "E1", DateTime.Parse("2000-01-01T11:00:00")),]
                            )
                        ]
                    ),
                },
            ],
        ];

    [Trait("Description", "2 Releases, 2 Environments, Keep 1")]
    public static IEnumerable<object[]> TestCase03(int retainReleaseCount) =>
        [
            [
                new TestDataContext
                (
                    [new Project("P1", "Project 1"),],
                    [
                        new Environment("E1", "Environment 1"),
                        new Environment("E2", "Environment 2"),
                    ],
                    [
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                        new Release("R2", "1.0.1", "P1", DateTime.Parse("2000-01-01T09:00:00")),
                    ],
                    [
                        new Deployment("D1", "R2", "E1", DateTime.Parse("2000-01-01T10:00:00")),
                        new Deployment("D2", "R1", "E2", DateTime.Parse("2000-01-01T11:00:00")),
                    ]
                ),
                1,  // RetainReleaseQuery.RetainReleaseCount
                // Expected value
                new []
                {
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E2", "Environment 2"),
                                new Release("R1", "1.0.0", "P1", DateTime.Parse("2000-01-01T08:00:00")),
                                [new Deployment("D2", "R1", "E2", DateTime.Parse("2000-01-01T11:00:00")),]
                            )
                        ]
                    ),
                    new TestDataGrouping<Release, RetainReleaseQuery.Result>
                    (
                        new Release("R2", "1.0.1", "P1", DateTime.Parse("2000-01-01T09:00:00")),
                        [
                            new RetainReleaseQuery.Result
                            (
                                new Project("P1", "Project 1"),
                                new Environment("E1", "Environment 1"),
                                new Release("R2", "1.0.1", "P1", DateTime.Parse("2000-01-01T09:00:00")),
                                [new Deployment("D1", "R2", "E1", DateTime.Parse("2000-01-01T10:00:00")),]
                            )
                        ]
                    )
                },
            ],
        ];
}
