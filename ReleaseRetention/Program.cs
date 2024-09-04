using Data.Context;
using Data.Queries;

var dataContext = new JsonFilesDataContext();

var releases = dataContext.Execute(new ReleaseHistoryQuery() { RetainReleaseCount = 4 });
Console.WriteLine($"releases = {releases.ToString("Releases")}");
