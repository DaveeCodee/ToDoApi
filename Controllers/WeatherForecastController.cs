using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.DatabaseContext;
using System.Globalization;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        public InMemoryContext _memoryDbContext;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            InMemoryContext inMemoryContext)
        {
            _logger = logger;
            _memoryDbContext = inMemoryContext;
        }

        [HttpGet(Name = "WeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = 1,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "WeatherForecast/{id}")]
        public async Task<ActionResult<WeatherForecast>> GetById(long id)
        {
            WeatherForecast weatherForecast = await _memoryDbContext.WeatherForecasts.FindAsync(id);

            if (weatherForecast == null)
            {
                return NotFound();
            }

            return weatherForecast;
        }

        [HttpPost(Name = "CreateWeatherForecast")]
        public async Task<WeatherForecast> CreateNewForecast(
            int year, int month, int day, int tempC, string summary)
        {
            var dateTicks = new DateTime(year, month, day, 0, 0, 0, 0,
                    new CultureInfo("en-US", false).Calendar).Ticks;
            var date = new DateTime(dateTicks);
            WeatherForecast newForecast =  new WeatherForecast
            {
                Date = DateOnly.FromDateTime(date),
                TemperatureC = tempC,
                Summary = summary
            };

            _memoryDbContext.WeatherForecasts.Add(newForecast);
            await _memoryDbContext.SaveChangesAsync();

            return newForecast;
        }
    }
}
