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
            return _memoryDbContext.WeatherForecasts.ToList();
           
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

        //I'm trying to create another function to edit it but i've not gotten a way around it


        //[HttpPut(Name = "EditWeatherForcast")]
        //public async Task<ActionResult<WeatherForecast>> EditForecast()
        //{



        //    await _memoryDbContext.SaveChangesAsync();
        //}
    }
}
