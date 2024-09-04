using Entities;
using Environment = Entities.Environment;

public interface IDataContext
{
    IEnumerable<Project> Projects { get; }
    IEnumerable<Environment> Environments { get; }
    IEnumerable<Release> Releases { get; }
    IEnumerable<Deployment> Deployments { get; }
}