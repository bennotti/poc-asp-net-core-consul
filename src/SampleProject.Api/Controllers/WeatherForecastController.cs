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
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _pipeline;
        public WeatherForecastController(IWeatherForecastService pipeline)
        {
            _pipeline = pipeline;
        }
        [HttpGet]
        [Route("")]
        public async Task<ListWheatherForecastResponseDto> Get()
        {
            return await _pipeline.Obter(new ListWheatherForecastRequestDto());
        }
    }
}
