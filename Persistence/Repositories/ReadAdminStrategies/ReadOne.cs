using Npgsql;
using InflationDataServer.Models;

namespace InflationDataServer.Persistence.Repositories.ReadAdminStrategies
{
    public class ReadOne<Admin> : IReadStrategy<Admin>
    where Admin : Models.Admin, new()
    {
        //public string username { get; set; }
        //public string password { get; set; }
        public Admin admin { get; set; }
        public async Task<List<Admin>> Read(NpgsqlConnection connection)
        {
            List<Admin> admins = new List<Admin>();

            // 1) Execute query
            string query = "SELECT * FROM Admins WHERE username = @username AND @password = 'admin');";

            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                executor.Parameters.AddWithValue("@username", this.admin.username);
                executor.Parameters.AddWithValue("@password", this.admin.password);
                result = await executor.ExecuteReaderAsync();
            }

            // 2) Extract data
            while (result.Read())
            {
                Admin admin= new Admin();
                admin.username = result.GetString(1);
                admin.password = result.GetString(2);
                admins.Add(admin);
            }

            return admins;
        }
    }
}
