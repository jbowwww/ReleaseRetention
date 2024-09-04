using Data.Context;
using Data.Entities;
using Environment = Data.Entities.Environment;

namespace Data.Queries;

public class ReleaseHistoryQuery : IQuery<IEnumerable<ReleaseHistoryQuery.Result>>
{
    public int RetainReleaseCount = 1;

    public IEnumerable<Result> Execute(IDataContext dataContext)
    {
        var r = (
            from p in dataContext.Projects
            from e in dataContext.Environments
            select new Result() {
                Project = p,
                Environment = e,
                Releases = (
                    from r in dataContext.Releases
                    join d in dataContext.Deployments on r.Id equals d.ReleaseId
                    where r.ProjectId == p.Id && d.EnvironmentId == e.Id
                    orderby d.DeployedAt descending
                    group d by r
                ).Take(RetainReleaseCount)
            }
        );
        return r;
    }

    public class Result
    {
        public required Project Project;
        public required Environment Environment;
        public required IEnumerable<IGrouping<Release, Deployment>> Releases;
        public override string ToString() =>
            $"{typeof(Result).Name}:\n{Project}\n{Environment}\n{Releases.ToString(nameof(Releases))}"
            .Replace("\n", "\n\t");
    }
}