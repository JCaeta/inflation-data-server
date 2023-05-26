using Microsoft.AspNetCore.Mvc;
using InflationDataServer.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using InflationDataServer.Tools;
using Newtonsoft.Json.Linq;
using InflationDataServer.Models.Responses;
using InflationDataServer.Models.Requests;

namespace InflationDataServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IConfiguration _configuration;

        public AdminController(ILogger<AdminController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("sign-in", Name = "SignInAdmin")]
        public async Task<AdminResponse> SignIn([FromBody] AdminRequest request)
        {
            AdminResponse response = new AdminResponse();
            AdminServices service = new AdminServices();
            if (await service.SignIn(request.username, request.password))
            {
                var jwtGenerator = new JwtManager(this._configuration);
                string token = jwtGenerator.GenerateJwtToken(request.username);
                response.message.id = 1;
                response.message.message = Helpers.StandardMessages[1];
                response.token = token;

            }
            else
            {
                response.message.id = -1;
                response.message.message = Helpers.StandardMessages[-1];
                response.token = null;
            }
            return response;
        }

        [HttpPost("change-password", Name = "ChangePasswordInAdmin")]
        public async Task<AdminResponse> ChangePassword([FromBody] AdminRequest request)
        {
            AdminResponse response = new AdminResponse();
            AdminServices service = new AdminServices();
            JwtManager jwtManager = new JwtManager(_configuration);

            if(request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                string username = jwtManager.GetUsernameInToken(request.token);

                if(await service.ChangePassword(username, request.password, request.oldPassword))
                {

                }


            } else
            {
                response.message.id = -6; //Access Denied: Invalid or expired token
                response.message.message = Helpers.StandardMessages[-6];
            }

            return response;
        }
    }
}



