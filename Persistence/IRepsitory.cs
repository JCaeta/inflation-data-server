using InflationDataServer.Persistence.Repositories;

namespace InflationDataServer.Persistence
{
    public interface IRepository<T>
    {
        Task<T> create(T item);
        Task<List<T>> read(IReadStrategy<T> strategy);

        Task<bool> update(T item);
        Task<bool> delete(T item);

    }
}
