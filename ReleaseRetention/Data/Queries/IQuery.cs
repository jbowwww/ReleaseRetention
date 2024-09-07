using ReleaseRetention.Data.Context;

namespace ReleaseRetention.Data.Queries;

public interface IQuery<out TQueryResult>
{
    TQueryResult Execute(IDataContext dataContext);
}