using Npgsql;
using InflationDataServer.Models;

namespace InflationDataServer.Persistence.Repositories.ReadInflationStrategies
{
    public class ReadLastYear<Inflation> : IReadStrategy<Inflation>
    where Inflation : Models.Inflation, new()
    {
        /*
         * Select a year of data starting from the latest entry and going backwards.
         */
        public async Task<List<Inflation>> Read(NpgsqlConnection connection)
        {
            List<Inflation> listInflation = new List<Inflation>();

            // 1) Execute query
            //string query = "SELECT * FROM Inflation;";
            string query = "SELECT* FROM inflation " +
            "WHERE date <= (SELECT date FROM inflation ORDER BY date DESC LIMIT 1) " +
            "AND EXTRACT(YEAR FROM date) >= EXTRACT(YEAR FROM(SELECT date FROM inflation ORDER BY date DESC LIMIT 1)) - 1 " +
            "ORDER BY date  DESC;";

            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                result = await executor.ExecuteReaderAsync();
            }

            // 2) Extract data
            while (result.Read())
            {
                int id = result.GetInt32(0);
                DateTime date = result.GetDateTime(1);
                float value = result.GetFloat(2);
                Inflation inflation = new Inflation();
                inflation.id = id;
                inflation.date = date;
                inflation.value = value;
                listInflation.Add(inflation);
            }

            return listInflation;
        }
    }
}
