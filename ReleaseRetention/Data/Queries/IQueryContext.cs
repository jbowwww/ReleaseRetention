namespace Data.Queries;

public interface IQueryContext
{
    public IDataContext DataContext { get; }
}