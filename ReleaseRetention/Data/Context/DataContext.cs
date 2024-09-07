using System.Collections.Generic;
using ReleaseRetention.Data.Entities;
using ReleaseRetention.Data.Queries;
using Environment = ReleaseRetention.Data.Entities.Environment;

namespace ReleaseRetention.Data.Context;

public class DataContext : IDataContext
{
    public IEnumerable<Project> Projects { get; protected set; } = [];

    public IEnumerable<Environment> Environments { get; protected set; } = [];

    public IEnumerable<Release> Releases { get; protected set; } = [];

    public IEnumerable<Deployment> Deployments { get; protected set; } = [];

    public TQueryResult Execute<TQueryResult>(IQuery<TQueryResult> query) => query.Execute(this);
}