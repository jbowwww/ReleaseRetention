using System;
using System.Collections.Generic;
using System.Linq;

using Data.Context;
using Data.Queries;

var retainReleaseCount = 2;

var dataContext = new JsonFilesDataContext();

var retainReleases = dataContext.Execute(new RetainReleaseQuery() { RetainReleaseCount = retainReleaseCount });

foreach (var release in retainReleases)
{
    Console.WriteLine($"{release.Key} kept because it was one of the {retainReleaseCount} most recent deployments in:\n\t" +
        string.Join("\n\t", release.Distinct(EqualityComparer<ReleaseRetention>.Create((rr1, rr2) =>
            rr1.Environment.Id == rr2.Environment.Id,
            perd => perd.Environment.GetHashCode()
        )).Select(perd => $"{perd.Environment} - deployed at {perd.Deployments.Where(
            d => d.EnvironmentId == perd.Environment.Id
        ).First().DeployedAt}")));
}
