
using System.Collections.Generic;
using System.Linq;

using ReleaseRetention.Data.Context;
using ReleaseRetention.Data.Entities;

namespace ReleaseRetention.Data.Queries;

public class RetainReleaseQuery : IQuery<IEnumerable<IGrouping<Release, RetainReleaseQuery.Result>>>
{
    public int RetainReleaseCount = 1;

    public IEnumerable<IGrouping<Release, Result>> Execute(IDataContext dataContext)
    {
        return (
            from p in dataContext.Projects
            from e in dataContext.Environments
            select (
                Project: p,
                Environment: e,
                Releases: (
                    from r in dataContext.Releases
                    join d in dataContext.Deployments on r.Id equals d.ReleaseId
                    where r.ProjectId == p.Id && d.EnvironmentId == e.Id
                    orderby d.DeployedAt descending
                    group d by r
                ).Take(RetainReleaseCount)
            )
        ).SelectMany(
            rh => rh.Releases,
            (rh, release) => new Result() {
                Project = rh.Project,
                Environment = rh.Environment,
                Release = release.Key,
                Deployments = release.AsEnumerable()
            }
        ).GroupBy(g => g.Release);
    }

    public record Result
    {
        public Project Project;
        public Environment Environment;
        public Release Release;
        public IEnumerable<Deployment> Deployments;
    };
}
