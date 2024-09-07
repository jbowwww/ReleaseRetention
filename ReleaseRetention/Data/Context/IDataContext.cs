using System.Collections.Generic;
using Data.Entities;
using Data.Queries;
using Environment = Data.Entities.Environment;

namespace Data.Context;

public interface IDataContext
{
    IEnumerable<Project> Projects { get; }
    IEnumerable<Environment> Environments { get; }
    IEnumerable<Release> Releases { get; }
    IEnumerable<Deployment> Deployments { get; }
    TQueryResult Execute<TQueryResult>(IQuery<TQueryResult> query);
}