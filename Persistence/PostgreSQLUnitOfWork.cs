using Npgsql;
using System.Reflection;
using InflationDataServer.Persistence.Repositories;
using InflationDataServer.Models;

namespace InflationDataServer.Persistence
{
    public class PostgreSQLUnitOfWork : IUnitOfWork
    {
        private string connectionString;
        private NpgsqlConnection connection;

        public PostgreSQLUnitOfWork(Dictionary<string, string> databaseInformation)
        {
            connectionString =
                "Server = " + databaseInformation["SERVER"] +
                "; User Id = " + databaseInformation["USER_ID"] +
                "; Password = " + databaseInformation["PASSWORD"] +
                "; Database = " + databaseInformation["DATABASE_NAME"];
        }

        public  void connect()
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public void disconnect()
        {
            connection.Close();
        }

        public async Task<Inflation> createInflation(Inflation inflation)
        {
            InflationRepository < Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            inflation = await inflationRepository.create(inflation);
            return inflation;
        }

        public async Task<List<Inflation>> readAllInflation()
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadAll<Inflation> strategy = new ReadAll<Inflation>();
            return await inflationRepository.read(strategy);
        }

        public async Task<List<Inflation>> readLastYearInflation()
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadLastYear<Inflation> strategy = new ReadLastYear<Inflation>();
            return await inflationRepository.read(strategy);
        }

        public async Task<List<Inflation>> readFromDateInflation(DateTime startDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadFromDate<Inflation> strategy = new ReadFromDate<Inflation>();
            strategy.StartDate = startDate;
            return await inflationRepository.read(strategy);
        }
        public async Task<List<Inflation>> readUntilDateInflation(DateTime endDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadUntilDate<Inflation> strategy = new ReadUntilDate<Inflation>();
            strategy.EndDate = endDate;
            return await inflationRepository.read(strategy);
        }

        public async Task<List<Inflation>> readIntervalInflation(DateTime startDate, DateTime endDate)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            ReadInterval<Inflation> strategy = new ReadInterval<Inflation>();
            strategy.StartDate = startDate;
            strategy.EndDate = endDate;
            return await inflationRepository.read(strategy);
        }

        public async Task<bool> updateInflation(Inflation inflation)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            return await inflationRepository.update(inflation);
        }

        public async Task<bool> deleteInflation(Inflation inflation)
        {
            InflationRepository<Inflation> inflationRepository = new InflationRepository<Inflation>(this.connection);
            return await inflationRepository.delete(inflation);
        }
    }
}
