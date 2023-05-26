using Npgsql;

namespace InflationDataServer.Persistence.Repositories
{
    public interface IUpdateStrategy<T>
    {
        Task<bool> Update(NpgsqlConnection connection);
    }
}
