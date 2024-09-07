using System.Collections.Generic;
using ReleaseRetention.Data.Entities;
using ReleaseRetention.Data.Queries;
using Environment = ReleaseRetention.Data.Entities.Environment;

namespace ReleaseRetention.Data.Context;

public interface IDataContext
{
    IEnumerable<Project> Projects { get; }
    IEnumerable<Environment> Environments { get; }
    IEnumerable<Release> Releases { get; }
    IEnumerable<Deployment> Deployments { get; }
    TQueryResult Execute<TQueryResult>(IQuery<TQueryResult> query);
}