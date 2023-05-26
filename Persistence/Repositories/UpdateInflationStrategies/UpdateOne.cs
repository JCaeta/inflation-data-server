using Npgsql;

namespace InflationDataServer.Persistence.Repositories.UpdateInflationStrategies
{
    public class UpdateOne<Inflation> : IUpdateStrategy<Inflation>
    where Inflation : InflationDataServer.Models.Inflation
    {
        public Inflation item { get; set; }
        public async Task<bool> Update(NpgsqlConnection connection)
        {
            string query = "UPDATE Inflation SET date = @date, value = @value WHERE id = @id";
            NpgsqlCommand executor = new NpgsqlCommand(query, connection);
            executor.Parameters.AddWithValue("@date", item.date);
            executor.Parameters.AddWithValue("@id", item.id);
            executor.Parameters.AddWithValue("@value", item.value);
            try
            {
                await executor.ExecuteReaderAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
