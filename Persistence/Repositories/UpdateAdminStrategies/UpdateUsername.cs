using Npgsql;

namespace InflationDataServer.Persistence.Repositories.UpdateAdminStrategies
{
    public class UpdateUsername<Admin> : IUpdateStrategy<Admin>
    where Admin : InflationDataServer.Models.Admin, new()
    {
        public Admin item { get; set; }
        public async Task<bool> Update(NpgsqlConnection connection)
        {
            string query = "UPDATE Admins SET username = @username WHERE password = @password";
            NpgsqlCommand executor = new NpgsqlCommand(query, connection);
            executor.Parameters.AddWithValue("@username", item.username);
            executor.Parameters.AddWithValue("@password", item.password);
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
