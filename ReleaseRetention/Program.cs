// Console.WriteLine(DataContext.);

// MainMenu.RunMainMenuLoop();

DataContext.Load();

Console.WriteLine($"Projects[0].Releases={DataContext.Projects.First().Releases.ToString("Releases")}");
Console.WriteLine($"Projects[1].Releases={DataContext.Projects.Skip(1).First().Releases.ToString("Releases")}");

var releases = RetainReleaseHistory(1);
Console.WriteLine($"releases = {releases.ToString("Releases")}");
// Console.WriteLine($"releases = {releases.ElementAt(1)}");
// Console.WriteLine($"releases = {releases.ElementAt(2)}");
// Console.WriteLine($"releases = {releases.ElementAt(3)}");

// "For each project/environment combination, keep n releases that have most recently been deployed, where n is the number of releases to keep.
// note: A release is considered to have "been deployed" if the release has one or more deployments."
// 'a is new { string ProjectId, string ProjectName, string EnvironmentId, string EnvironmentName, List Releases }
static IEnumerable<ReleaseHistory> RetainReleaseHistory(int retainReleaseCount = 1)
{
    var r = (
        from p in DataContext.Projects
        from e in DataContext.Environments
        select new ReleaseHistory() {
            Project = p,
            // ProjectId = p.Id,
            // ProjectName = p.Name,
            Environment = e,
            // EnvironmentId = e.Id,
            // EnvironmentName = e.Name,
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