using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleProject.Core.Dto.WeatherForecast;
using SampleProject.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Api.Controllers
{
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult Status() => Ok();
    }
}
