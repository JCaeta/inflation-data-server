using InflationDataServer.Models;
using InflationDataServer.Persistence;
using InflationDataServer.ViewModels;

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
        public async Task<InflationResponse> createInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            InflationResponse response = new InflationResponse();
            try
            {
                // Check if inflation is not null
                if(inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    Inflation inf = await unitOfWork.createInflation(inflation);
                    unitOfWork.disconnect();

                    // Build response
                    response.inflationList.Add(inf);
                    response.message.message = Helpers.StandardMessages[1]; // Succeeded
                    response.message.id = 1;
                }
                else
                {
                    response.message.id = -3;
                    response.message.message = Helpers.StandardMessages[-3]; // Inflation is null
                }
            }
            catch(Exception ex)
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
            }
            return response;
        }

        public async Task<InflationResponse> readInflation(DateTime? startDate, DateTime? endDate)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            InflationResponse response = new InflationResponse();

            try
            {
                // Select option read
                if (startDate == null)
                {
                    if (endDate == null)
                    {
                        // Read a year of data
                        unitOfWork.connect();
                        response.inflationList = await unitOfWork.readAllInflation();
                        unitOfWork.disconnect();
                    }else
                    {
                        // Read until end date
                        unitOfWork.connect();
                        response.inflationList = await unitOfWork.readUntilDateInflation(endDate.Value);
                        unitOfWork.disconnect();
                    }
                }
                else
                {
                    if (endDate == null)
                    {
                        // Read a year of data
                        unitOfWork.connect();
                        response.inflationList = await unitOfWork.readFromDateInflation(startDate.Value);
                        unitOfWork.disconnect();

                    }else
                    {
                        // Read interval
                        unitOfWork.connect();
                        response.inflationList = await unitOfWork.readIntervalInflation(startDate.Value, endDate.Value);
                        unitOfWork.disconnect();
                    }    
                }
                response.message.message = Helpers.StandardMessages[1];
                response.message.id = 1;
            }
            catch (Exception ex)
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
            }
            return response;
        }

        public async Task<InflationResponse> updateInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            InflationResponse response = new InflationResponse();
            try
            {
                // Check if inflation is not null
                if (inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    bool result = await unitOfWork.updateInflation(inflation);
                    unitOfWork.disconnect();
                    if (result)
                    {
                        // Build response
                        response.inflationList.Add(inflation);
                        response.message.message = Helpers.StandardMessages[1]; // Succeeded
                        response.message.id = 1;
                    }else
                    {
                        response.message.id = -4;
                        response.message.message = Helpers.StandardMessages[-4]; // Inflation updating failed
                    }
                }
                else
                {
                    response.message.id = -3;
                    response.message.message = Helpers.StandardMessages[-3]; // Inflation is null
                }
            }
            catch (Exception ex)
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
            }
            return response;
        }


        public async Task<InflationResponse> deleteInflation(Inflation? inflation)
        {
            PostgreSQLUnitOfWork unitOfWork = new PostgreSQLUnitOfWork(DatabaseInformation.Information);
            InflationResponse response = new InflationResponse();
            try
            {
                // Check if inflation is not null
                if (inflation != null)
                {
                    // Perfrom operation
                    unitOfWork.connect();
                    bool result = await unitOfWork.deleteInflation(inflation);
                    unitOfWork.disconnect();
                    if (result)
                    {
                        // Build response
                        response.inflationList.Add(inflation);
                        response.message.message = Helpers.StandardMessages[1]; // Succeeded
                        response.message.id = 1;
                    }
                    else
                    {
                        response.message.id = -4;
                        response.message.message = Helpers.StandardMessages[-5]; // Inflation deletion failed
                    }
                }
                else
                {
                    response.message.id = -3;
                    response.message.message = Helpers.StandardMessages[-3]; // Inflation is null
                }
            }
            catch (Exception ex)
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
            }
            return response;
        }
    }
}
