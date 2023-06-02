using Npgsql;
using System.Reflection;
using InflationDataServer.Persistence.Repositories;
using InflationDataServer.Models;
using InflationDataServer.Persistence.Repositories.ReadInflationStrategies;
using InflationDataServer.Persistence.Repositories.ReadAdminStrategies;
using InflationDataServer.Persistence.Repositories.UpdateInflationStrategies;
using InflationDataServer.Persistence.Repositories.UpdateAdminStrategies;

namespace InflationDataServer.Persistence
{
    public class PostgreSQLUnitOfWork : IUnitOfWork
    {
        private string connectionString;
        private NpgsqlConnection connection;

        public PostgreSQLUnitOfWork(Dictionary<string, string> databaseInformation)
        {

            connectionString =
                "Server=" + databaseInformation["SERVER"] +
                ";Port=" + databaseInformation["PORT"] +
                ";User Id=" + databaseInformation["USER_ID"] +
                ";Password=" + databaseInformation["PASSWORD"] +
                ";Database=" + databaseInformation["DATABASE_NAME"];

            Console.WriteLine(connectionString);
        }

        public  void connect()
        {
            connection = new NpgsqlConnection();
            connection.Open();
        }

        public void disconnect()
        {
            connection.Close();
        }

        public async Task<Inflation> CreateInflation(Inflation inflation)
        {
            InflationRepository < Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            inflation = await inflationRepository.Create(inflation);
            return inflation;
        }

        public async Task<List<Inflation>> ReadAllInflation()
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadAll<Inflation> strategy = new ReadAll<Inflation>();
            return await inflationRepository.Read(strategy);
        }

        public async Task<List<Inflation>> ReadLastYearInflation()
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadLastYear<Inflation> strategy = new ReadLastYear<Inflation>();
            return await inflationRepository.Read(strategy);
        }

        public async Task<List<Inflation>> ReadFromDateInflation(DateTime startDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadFromDate<Inflation> strategy = new ReadFromDate<Inflation>();
            strategy.StartDate = startDate;
            return await inflationRepository.Read(strategy);
        }
        public async Task<List<Inflation>> ReadUntilDateInflation(DateTime endDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadUntilDate<Inflation> strategy = new ReadUntilDate<Inflation>();
            strategy.EndDate = endDate;
            return await inflationRepository.Read(strategy);
        }

        public async Task<List<Inflation>> ReadIntervalInflation(DateTime startDate, DateTime endDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadInterval<Inflation> strategy = new ReadInterval<Inflation>();
            strategy.StartDate = startDate;
            strategy.EndDate = endDate;
            return await inflationRepository.Read(strategy);
        }

        public async Task<bool> UpdateInflation(Inflation inflation)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            UpdateOne<Inflation> strategy = new UpdateOne<Inflation>();
            strategy.item = inflation;
            return await inflationRepository.Update(strategy);
        }

        public async Task<bool> DeleteInflation(Inflation inflation)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            return await inflationRepository.Delete(inflation);
        }

        public async Task<bool> SignInAdmin(string username, string password)
        {
            AdminRepository<Admin> adminRepository = new AdminRepository<Admin>(this.connection);
            return await adminRepository.SignIn(username, password);
        }

        public async Task<bool> ExistsAdmin(Admin admin)
        {
            AdminRepository<Admin> adminRepository = new AdminRepository<Admin>(this.connection);
            ReadOne<Admin> strategy = new ReadOne<Admin>();
            strategy.admin = admin;
            List<Admin> result = await adminRepository.Read(strategy);
            if(result.Count > 0) { return true; }
            return false;
        }

        public async Task<bool> UpdateAdminPassword(Admin admin)
        {
            AdminRepository<Admin> adminRepository = new AdminRepository<Admin>(this.connection);
            UpdateUsername<Admin> strategy = new UpdateUsername<Admin>();
            strategy.item = admin;
            return await adminRepository.Update(strategy);
        }
    }
}
