using Npgsql;
using InflationDataServer.Models;

namespace InflationDataServer.Persistence.Repositories
{
    public class InflationRepository<Inflation> : IRepository<Inflation>
    where Inflation : InflationDataServer.Models.Inflation
    {
        private NpgsqlConnection connection;
        private IReadStrategy<Inflation> readStrategy;

        public InflationRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Inflation> create(Inflation item)
        {
            string query = "insert into inflation(date, value) values(@date, @value) returning id;";
            NpgsqlCommand executor = new NpgsqlCommand(query, this.connection);
            executor.Parameters.AddWithValue("@date", item.Date);
            executor.Parameters.AddWithValue("@value", item.Value);
            //int result = int.Parse(executor.ExecuteScalarAsync().ToString());
            var result = await executor.ExecuteScalarAsync();
            int id = int.Parse(result.ToString());
            item.Id = id;
            return item;
        }

        public async Task<bool> delete(Inflation item)
        {
            string query = "delete from Inflation where id=@id;";
            NpgsqlCommand executor = new NpgsqlCommand(query, this.connection);
            executor.Parameters.AddWithValue("id", item.Id);
            try
            {
                executor.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Inflation>> read(IReadStrategy<Inflation> strategy)
        {
            return await strategy.read(this.connection);
        }

        public async Task<bool> update(Inflation item)
        {
            string query = "UPDATE Inflation SET date = @date, value = @value WHERE id = @id";
            NpgsqlCommand executor = new NpgsqlCommand(query, this.connection);
            executor.Parameters.AddWithValue("@date", item.Date);
            executor.Parameters.AddWithValue("@id", item.Id);
            executor.Parameters.AddWithValue("@value", item.Value);
            try
            {
                executor.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
