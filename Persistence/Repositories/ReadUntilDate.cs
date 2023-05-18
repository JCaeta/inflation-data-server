using Npgsql;

namespace InflationDataServer.Persistence.Repositories
{
    public class ReadUntilDate<Inflation> : IReadStrategy<Inflation>
    where Inflation : InflationDataServer.Models.Inflation, new()
    {
        public DateTime EndDate { get; set; }
        public async Task<List<Inflation>> read(NpgsqlConnection connection)
        {
            List<Inflation> listInflation = new List<Inflation>();

            // 1) Execute query
            //string query = "SELECT * FROM Inflation;";
            string query = "SELECT * FROM inflation " +
            "WHERE date <= @endDate " +
            "ORDER BY date DESC;";

            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                executor.Parameters.AddWithValue("@endDate", this.EndDate);
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
