using Data.Context;

namespace Data.Queries;

public interface IQuery<TQueryResult>
{
    TQueryResult Execute(IDataContext dataContext);
}