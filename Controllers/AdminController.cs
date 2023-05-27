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
                response.message.id = 1; // Succeeded
                response.message.message = Helpers.StandardMessages[1];
                response.token = token;

            }
            else
            {
                response.message.id = -1; // Request failed to complete
                response.message.message = Helpers.StandardMessages[-1];
                response.token = null;
            }
            return response;
        }

        [HttpPost("change-password", Name = "ChangePasswordAdmin")]
        public async Task<AdminResponse> ChangePassword([FromBody] AdminRequest request)
        {
            AdminResponse response = new AdminResponse();
            AdminServices service = new AdminServices();
            JwtManager jwtManager = new JwtManager(_configuration);

            // Validate token
            if(request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                string username = jwtManager.GetUsernameInToken(request.token);

                // Validate old password
                if(await service.ValidatePassword(username, request.oldPassword))
                {
                    // Change password
                    if (await service.ChangePassword(username, request.oldPassword, request.oldPassword))
                    {
                        response.message.id = 1; //Succeeded
                        response.message.message = Helpers.StandardMessages[1];

                        string newToken = jwtManager.GenerateJwtToken(username);
                        response.token = newToken;
                    }
                    else
                    {
                        response.message.id = -1; // Request failed to complete
                        response.message.message = Helpers.StandardMessages[-1];

                        string newToken = jwtManager.GenerateJwtToken(username);
                        response.token = newToken;
                    }
                }else
                {
                    response.message.id = -7; //Invalid password
                    response.message.message = Helpers.StandardMessages[-7];
                }
            } else
            {
                response.message.id = -6; //Access Denied: Invalid or expired token
                response.message.message = Helpers.StandardMessages[-6];
            }
            return response;
        }

        [HttpPost("change-username", Name = "ChangeUsernameAdmin")]
        public async Task<AdminResponse> ChangeUsername([FromBody] AdminRequest request)
        {
            AdminResponse response = new AdminResponse();
            AdminServices service = new AdminServices();
            JwtManager jwtManager = new JwtManager(_configuration);

            // Validate token
            if (request.token != null && jwtManager.ValidateJwtToken(request.token))
            {
                string oldUsername = jwtManager.GetUsernameInToken(request.token);

                // Validate password
                if(await service.ValidatePassword(oldUsername, request.password))
                {
                    // Change username
                    if (await service.ChangeUsername(request.username, request.password))
                    {
                        response.message.id = 1; // Succeeded
                        response.message.message = Helpers.StandardMessages[1];

                        string newToken = jwtManager.GenerateJwtToken(request.username);
                        response.token = newToken;

                    }
                    else
                    {
                        response.message.id = -1; //Request failed to complete
                        response.message.message = Helpers.StandardMessages[-1];

                        string newToken = jwtManager.GenerateJwtToken(request.username);
                        response.token = newToken;
                    }
                }
                else
                {
                    response.message.id = -7; //Invalid password
                    response.message.message = Helpers.StandardMessages[-7];
                }
            }
            else
            {
                response.message.id = -6; //Access Denied: Invalid or expired token
                response.message.message = Helpers.StandardMessages[-6];
            }
            return response;
        }
    }
}



