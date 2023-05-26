using Microsoft.AspNetCore.Mvc;
using InflationDataServer.Services;
using InflationDataServer.Models.Responses;
using InflationDataServer.Models.Requests;
using InflationDataServer.Models;
using InflationDataServer.Tools;

namespace InflationDataServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChartsController
    {
        private readonly ILogger<ChartsController> _logger;

        public ChartsController(ILogger<ChartsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("get-data", Name = "GetDataCharts")]
        public async Task<ChartsResponse> GetData([FromBody] ChartsRequest request)
        {
            ChartsService chartsService = new ChartsService();
            ChartsData data = await chartsService.readChartsData(request.startDate, request.endDate);
            ChartsResponse response = new ChartsResponse();
            response.message.id = 1;
            response.message.message = Helpers.StandardMessages[1];
            response.data = data;
            return response;
        }
    }
}
