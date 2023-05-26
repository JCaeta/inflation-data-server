using Npgsql;

namespace InflationDataServer.Persistence.Repositories.ReadInflationStrategies
{
    public class ReadInterval<Inflation> : IReadStrategy<Inflation>
    where Inflation : Models.Inflation, new()
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public async Task<List<Inflation>> Read(NpgsqlConnection connection)
        {
            List<Inflation> listInflation = new List<Inflation>();

            // 1) Execute query
            //string query = "SELECT * FROM Inflation;";
            string query = "SELECT * FROM inflation " +
            "WHERE date >= @startDate and date <= @endDate " +
            "ORDER BY date DESC;";

            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                executor.Parameters.AddWithValue("@startDate", StartDate);
                executor.Parameters.AddWithValue("@endDate", EndDate);
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
