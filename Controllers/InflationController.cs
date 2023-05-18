﻿using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using InflationDataServer.Models;
using InflationDataServer.Persistence;
using InflationDataServer.Services;
using InflationDataServer.ViewModels;


namespace InflationDataServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InflationController
    {
        private readonly ILogger<InflationController> _logger;

        public InflationController(ILogger<InflationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("create", Name = "CreateInflation")]
        public async Task<InflationResponse> Create([FromBody]InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            return await inflationService.createInflation(request.inflation);
        }

        [HttpPost("read", Name = "ReadInflation")]
        public async Task<InflationResponse> Read([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            return await inflationService.readInflation(request.startDate, request.endDate);
        }

        [HttpPut("update", Name = "UpdateInflation")]
        public async Task<InflationResponse> Update([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            return await inflationService.updateInflation(request.inflation);
        }

        [HttpDelete("delete", Name = "DeleteInflation")]
        public async Task<InflationResponse> Delete([FromBody] InflationRequest request)
        {
            InflationService inflationService = new InflationService();
            return await inflationService.deleteInflation(request.inflation);
        }

    }
}
