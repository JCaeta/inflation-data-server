namespace InflationDataServer.Persistence.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Create(T item);
        Task<List<T>> Read(IReadStrategy<T> strategy);

        Task<bool> Update(IUpdateStrategy<T> strategy);
        Task<bool> Delete(T item);

    }
}
