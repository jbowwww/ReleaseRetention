global using ReleaseRetention =
(
    Data.Entities.Project Project,
    Data.Entities.Environment Environment,
    Data.Entities.Release Release,
    System.Collections.Generic.IEnumerable<Data.Entities.Deployment> Deployments
);

using System.Collections.Generic;
using System.Linq;
using Data.Context;
using Data.Entities;

namespace Data.Queries;
public class RetainReleaseQuery : IQuery<IEnumerable<IGrouping<Release, ReleaseRetention>>>
{
    public int RetainReleaseCount = 1;

    public IEnumerable<IGrouping<Release, ReleaseRetention>> Execute(IDataContext dataContext)
    {
        var r = (
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
            (rh, release) => (
                rh.Project,
                rh.Environment,
                Release: release.Key,
                Deployments: release.AsEnumerable()
            )
        ).GroupBy(g => g.Release);
        return r;
    }
}
