using System.Collections.Generic;
using Data.Entities;
using Data.Queries;
using Environment = Data.Entities.Environment;

namespace Data.Context;

public class DataContext : IDataContext
{
    public IEnumerable<Project> Projects { get; protected set; } = [];

    public IEnumerable<Environment> Environments { get; protected set; } = [];

    public IEnumerable<Release> Releases { get; protected set; } = [];

    public IEnumerable<Deployment> Deployments { get; protected set; } = [];

    public TQueryResult Execute<TQueryResult>(IQuery<TQueryResult> query) => query.Execute(this);
}