using Npgsql;
using InflationDataServer.Models;
using InflationDataServer.Persistence.Repositories;

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

        public async Task<Inflation> Create(Inflation item)
        {
            string query = "insert into inflation(date, value) values(@date, @value) returning id;";

            DateTimeOffset dateWithoutTimeZone = new DateTimeOffset(item.date, TimeSpan.Zero);
            NpgsqlCommand executor = new NpgsqlCommand(query, this.connection);
            executor.Parameters.AddWithValue("@date", dateWithoutTimeZone);
            executor.Parameters.AddWithValue("@value", item.value);
            var result = await executor.ExecuteScalarAsync();
            int id = int.Parse(result.ToString());
            item.id = id;
            return item;
        }

        public async Task<bool> Delete(Inflation item)
        {
            string query = "delete from Inflation where id=@id;";
            NpgsqlCommand executor = new NpgsqlCommand(query, this.connection);
            executor.Parameters.AddWithValue("id", item.id);
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

        public async Task<List<Inflation>> Read(IReadStrategy<Inflation> strategy)
        {
            return await strategy.Read(this.connection);
        }

        public async Task<bool> Update(IUpdateStrategy<Inflation> strategy)
        {

            return await strategy.Update(this.connection);   
        }
    }
}
