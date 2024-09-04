using Data.Entities;
using Environment = Data.Entities.Environment;

namespace Data;

public interface IDataContext
{
    IEnumerable<Project> Projects { get; }
    IEnumerable<Environment> Environments { get; }
    IEnumerable<Release> Releases { get; }
    IEnumerable<Deployment> Deployments { get; }
}