using Microsoft.Extensions.Logging;
using SampleProject.Core.Dto.WeatherForecast;
using SampleProject.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infra.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastService> _logger;
        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }

        public async Task<ListWheatherForecastResponseDto> Obter(ListWheatherForecastRequestDto request)
        {
            _logger.LogInformation("Inicio metodo obter");
            Random rng = new Random();
            var lista = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.LogInformation("passou na serviço");

            return await Task.FromResult(new ListWheatherForecastResponseDto {
                Lista = lista
            });
        }
    }
}
