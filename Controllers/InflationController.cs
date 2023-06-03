using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using InflationDataServer.Models;
using InflationDataServer.Persistence;
using InflationDataServer.Services;
using InflationDataServer.Tools;
using InflationDataServer.Models.Responses;
using InflationDataServer.Models.Requests;

namespace InflationDataServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InflationController
    {
        private readonly ILogger<InflationController> _logger;
        private readonly IConfiguration _configuration;

        public InflationController(ILogger<InflationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("create", Name = "CreateInflation")]
        public async Task<InflationResponse> Create([FromBody]InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            InflationResponse response = new InflationResponse();
            JwtManager jwtManager = new JwtManager(_configuration);
            if(request.token != null && jwtManager.ValidateJwtToken(request.token)) {
                Inflation? inflation = await inflationService.createInflation(request.inflation);
                if(inflation != null)
                {
                    response.message.id = 1;
                    response.message.message = Helpers.StandardMessages[1];
                    response.inflationList.Add(inflation);
                } else
                {
                    response.message.id = -1;
                    response.message.message = Helpers.StandardMessages[-1]; //Request failed to complete
                }

                string username = jwtManager.GetUsernameInToken(request.token);
                string newToken = jwtManager.GenerateJwtToken(username);
                response.token = newToken;
                return response;

            } else
            {
                response.message.id = -6; //Access Denied: Invalid or expired token
                response.message.message = Helpers.StandardMessages[-6];
                response.token = null;
                return response;
            }
        }

        [HttpPost("read", Name = "ReadInflation")]
        public async Task<InflationResponse> Read([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            InflationResponse response = new InflationResponse();
            JwtManager jwtManager = new JwtManager(_configuration);

            try
            {
                response.inflationList = await inflationService.readInflation(request.startDate, request.endDate);
                response.message.message = Helpers.StandardMessages[1];
                response.message.id = 1;
            }
            catch (Exception ex)
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
            }

            if (request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                string username = jwtManager.GetUsernameInToken(request.token);
                string newToken = jwtManager.GenerateJwtToken(username);
                response.token = newToken;
            }
                return response;
        }

        [HttpPut("update", Name = "UpdateInflation")]
        public async Task<InflationResponse> Update([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            JwtManager jwtManager = new JwtManager(_configuration);
            InflationResponse response = new InflationResponse();

            if (request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                string username = jwtManager.GetUsernameInToken(request.token);
                string newToken = jwtManager.GenerateJwtToken(username);
                if (await inflationService.updateInflation(request.inflation))
                {
                    response.message.id = 1;
                    response.message.message = Helpers.StandardMessages[1];
                    response.token = newToken;

                } else
                {

                    response.message.id = -1;
                    response.message.message = Helpers.StandardMessages[-1];
                    response.token = newToken;
                }
                return response;
            }
            else
            {
                response.message.id = -6; //Access Denied: Invalid or expired token"
                response.message.message = Helpers.StandardMessages[-6];
                response.token = null;
                return response;
            }
        }

        [HttpDelete("delete", Name = "DeleteInflation")]
        public async Task<InflationResponse> Delete([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            JwtManager jwtManager = new JwtManager(_configuration);
            InflationResponse response = new InflationResponse();

            if (request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                if (await inflationService.deleteInflation(request.inflation))
                {
                    string username = jwtManager.GetUsernameInToken(request.token);
                    string newToken = jwtManager.GenerateJwtToken(username);
                    response.message.id = 1;
                    response.message.message = Helpers.StandardMessages[1];
                    response.token = newToken;

                }
                else
                {
                    response.message.id = -1;
                    response.message.message = Helpers.StandardMessages[-1];
                    response.token = null;
                }
                return response;
            }
            else
            {
                response.message.id = -5;
                response.message.message = Helpers.StandardMessages[-5];
                response.token = null;
                return response;
            }
        }
    }
}
