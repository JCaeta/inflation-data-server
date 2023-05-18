using Npgsql;
using System.Reflection;

namespace InflationDataServer.Persistence.Repositories
{
    public class ReadAll<Inflation> : IReadStrategy<Inflation>
    where Inflation : InflationDataServer.Models.Inflation, new()
    {
        public async Task<List<Inflation>> read(NpgsqlConnection connection)
        {
            List<Inflation> listInflation = new List<Inflation>();

            // 1) Execute query
            string query = "SELECT * FROM Inflation ORDER BY date DESC;";
            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                result = await executor.ExecuteReaderAsync();
            }

            // 2) Extract data
            while (result.Read())
            {
                System.Int32 id = result.GetInt32(0);
                DateTime date = result.GetDateTime(1);
                float value = result.GetFloat(2);
                Inflation inflation = new Inflation();
                inflation.Id = id;
                inflation.Date = date;
                inflation.Value = value;
                listInflation.Add(inflation);
            }

            return listInflation;
        }
    }
}
