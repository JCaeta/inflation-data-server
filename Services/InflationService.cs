using InflationDataServer.Models;
using InflationDataServer.Models.Responses;
using InflationDataServer.Persistence;

namespace InflationDataServer.Services
{
    public class InflationService
    {
        // At the moment we have data only since 2011
        //DateTime minDate = new DateTime(2011, 1, 1);

        /**
            Standard messages
            id: message
            1: Succeeded
            -1: Request failed to complete
            -2: Communication with server failed
            -3: Inflation is null
            -4: Inflation updating failed
         */
        public async Task<Inflation?> createInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.GetDbInfo());
            try
            {
                // Check if inflation is not null
                if(inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    Inflation inf = await unitOfWork.CreateInflation(inflation);
                    unitOfWork.disconnect();
                    return inf;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Inflation>> readInflation(DateTime? startDate, DateTime? endDate)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.GetDbInfo());
            List<Inflation> inflations = new List<Inflation>();

            // Select option read
            if (startDate == null)
            {
                if (endDate == null )
                {
                    // Read a year of data
                    unitOfWork.connect();
                    inflations = await unitOfWork.ReadAllInflation();
                    unitOfWork.disconnect();
                }else
                {
                    // Read until end date
                    unitOfWork.connect();
                    inflations = await unitOfWork.ReadUntilDateInflation(endDate.Value);
                    unitOfWork.disconnect();
                }
            }
            else
            {
                if (endDate == null)
                {
                    // Read a year of data
                    unitOfWork.connect();
                    inflations = await unitOfWork.ReadFromDateInflation(startDate.Value);
                    unitOfWork.disconnect();

                }else
                {
                    // Read interval
                    unitOfWork.connect();
                    inflations = await unitOfWork.ReadIntervalInflation(startDate.Value, endDate.Value);
                    unitOfWork.disconnect();
                }    
            }
            return inflations;
        }

        public async Task<bool> updateInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.GetDbInfo());
            InflationResponse response = new InflationResponse();
            try
            {
                // Check if inflation is not null
                if (inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    bool result = await unitOfWork.UpdateInflation(inflation);
                    unitOfWork.disconnect();

                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> deleteInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.GetDbInfo());
            InflationResponse response = new InflationResponse();
            try
            {
                // Check if inflation is not null
                if (inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    bool result = await unitOfWork.DeleteInflation(inflation);
                    unitOfWork.disconnect();
                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
