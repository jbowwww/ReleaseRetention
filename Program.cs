// Console.WriteLine(DataContext.);

// MainMenu.RunMainMenuLoop();

DataContext.Load();

Console.WriteLine($"Projects[0].Releases={DataContext.Projects.First().Releases}");

var releases = RetainReleaseHistory(1);
Console.WriteLine($"releases = {releases.ToString("Releases")}");

// "For each project/environment combination, keep n releases that have most recently been deployed, where n is the number of releases to keep.
// note: A release is considered to have "been deployed" if the release has one or more deployments."
static IEnumerable<(Release, Deployment)> RetainReleaseHistory(int n = 1)
{
    var releases =
        from r in DataContext.Releases
        join d in DataContext.Deployments
            on r.Id equals d.ReleaseId
        select (r, d);
    return releases;
}