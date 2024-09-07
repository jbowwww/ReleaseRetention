using Data.Context;

namespace Data.Queries;

public interface IQuery<out TQueryResult>
{
    TQueryResult Execute(IDataContext dataContext);
}