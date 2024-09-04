namespace Data.Queries;

public class QueryContext : IQueryContext
{
    public IDataContext DataContext { get; }
    public QueryContext(IDataContext dataContext)
    {
        DataContext = dataContext;
    }

    // "For each project/environment combination, keep n releases that have most recently been deployed, where n is the number of releases to keep.
    // note: A release is considered to have "been deployed" if the release has one or more deployments."
    // 'a is new { string ProjectId, string ProjectName, string EnvironmentId, string EnvironmentName, List Releases }
    public IEnumerable<ReleaseHistory> RetainReleaseHistory(int retainReleaseCount = 1)
    {
        var r = (
            from p in DataContext.Projects
            from e in DataContext.Environments
            select new ReleaseHistory() {
                Project = p,
                Environment = e,
                Releases = (
                    from r in DataContext.Releases
                    join d in DataContext.Deployments on r.Id equals d.ReleaseId
                    where r.ProjectId == p.Id && d.EnvironmentId == e.Id
                    orderby d.DeployedAt descending
                    group d by r
                ).Take(retainReleaseCount)
            }
        );
        return r;
    }
}