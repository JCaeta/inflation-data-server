

using InflationDataServer.Models;
using Npgsql;

namespace InflationDataServer.Persistence.Repositories
{
    public class AdminRepository<Admin> : IRepository<Admin>
    where Admin : InflationDataServer.Models.Admin
    {
        private NpgsqlConnection connection;
        private IReadStrategy<Inflation> readStrategy;

        public AdminRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Admin> Create(Admin item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Admin item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Admin>> Read(IReadStrategy<Admin> strategy)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(IUpdateStrategy<Admin> strategy)
        {
            return await strategy.Update(this.connection);
        }


        public async Task<bool> SignIn(string username, string password)
        {
            // 1) Execute query
            string query = "SELECT EXISTS(SELECT 1 FROM Admins WHERE username = @username AND @password = 'admin');";


            NpgsqlDataReader result;
            using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
            {
                executor.Parameters.AddWithValue("@username", username);
                executor.Parameters.AddWithValue("@password", password);
                result = await executor.ExecuteReaderAsync();
            }

            try
            {
                // 2) Extract data
                while (result.Read())
                {
                    return result.GetBoolean(1);
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //public async Task<bool> PasswordIsEqualTo(string password)
        //{
        //    string query = "SELECT EXISTS (SELECT 1 FROM Admins WHERE password = @password) AS password_match;";

        //    NpgsqlDataReader result;
        //    using (NpgsqlCommand executor = new NpgsqlCommand(query, connection))
        //    {
        //        executor.Parameters.AddWithValue("@password", password);
        //        result = await executor.ExecuteReaderAsync();
        //    }

        //    try
        //    {
        //        // 2) Extract data
        //        while (result.Read())
        //        {
        //            return result.GetBoolean(1);
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
