using System;
using System.Collections.Generic;
using System.Linq;

using ReleaseRetention.Data.Context;
using ReleaseRetention.Data.Queries;

var retainReleaseCount = 2;

var dataContext = new JsonFilesDataContext();

var retainReleases = dataContext.Execute(new RetainReleaseQuery() { RetainReleaseCount = retainReleaseCount });

foreach (var release in retainReleases)
{
    Console.WriteLine($"{release.Key} kept because it was one of the {retainReleaseCount} most recent deployments in:\n\t" +
        string.Join("\n\t", release.Distinct(EqualityComparer<RetainReleaseQuery.Result>.Create((rr1, rr2) =>
            rr1?.Environment.Id == rr2?.Environment.Id,
            rr => rr.Environment.GetHashCode()
        )).Select(rr => $"{rr.Environment}".PadRight(56) + $" - deployed at {rr.Deployments.Where(
            d => d.EnvironmentId == rr.Environment.Id
        ).First().DeployedAt}")));
}
