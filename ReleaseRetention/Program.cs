var dataContext = new JsonFilesDataContext();
var queryContext = new QueryContext(dataContext);

var releases = queryContext.RetainReleaseHistory(1);
Console.WriteLine($"releases = {releases.ToString("Releases")}");
