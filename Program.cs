// Console.WriteLine(DataContext.);

// MainMenu.RunMainMenuLoop();

using System.Collections;

DataContext.Load();

Console.WriteLine($"Projects[0].Releases={DataContext.Projects.First().Releases.ToString("Releases")}");

var releases = RetainReleaseHistory(3);
Console.WriteLine($"releases = {releases.ToString("Releases")}");

// "For each project/environment combination, keep n releases that have most recently been deployed, where n is the number of releases to keep.
// note: A release is considered to have "been deployed" if the release has one or more deployments."
static IEnumerable RetainReleaseHistory(int n = 1)
{
    return (
        from p in DataContext.Projects
        from e in DataContext.Environments
        let deployHistory =
            from r in DataContext.Releases
            join d in DataContext.Deployments on r.Id equals d.ReleaseId
            where r.ProjectId == p.Id && d.EnvironmentId == e.Id
            group d by r into g
            select new
            {
                Release = g.Key,
                LastDeployedAt = g.Max(d => d.DeployedAt) // Most recent deployment date
            }
        where deployHistory.Any() // Filter combinations that have at least one deployed release
        select new
        {
            ProjectId = p.Id,
            ProjectName = p.Name,
            EnvironmentId = e.Id,
            EnvironmentName = e.Name,
            Releases = deployHistory
                .OrderByDescending(r => r.LastDeployedAt) // Sort by most recent deployment date
                .Take(n) // Keep only the top `n` releases
                .Select(r => r.Release) // Select the release info
                .ToList()
        }
    );
}