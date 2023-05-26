using Npgsql;

namespace InflationDataServer.Persistence.Repositories.UpdateAdminStrategies
{
    public class UpdatePassword<Admin> : IUpdateStrategy<Admin>
    where Admin : InflationDataServer.Models.Admin, new()
    {
        public Admin item { get; set; }
        public async Task<bool> Update(NpgsqlConnection connection)
        {
            string query = "UPDATE Admins SET password = @password WHERE username = @username";
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
