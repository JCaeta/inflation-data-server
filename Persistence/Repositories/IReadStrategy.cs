using Npgsql;

namespace InflationDataServer.Persistence.Repositories
{
    public interface IReadStrategy<T>
    {
        Task<List<T>> Read(NpgsqlConnection connection);
    }
}
