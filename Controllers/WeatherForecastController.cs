using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.DatabaseContext;
using System.Globalization;
using System.Linq;

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
        [HttpGet("ReverseForecast")]
        public IEnumerable<WeatherForecast> GetReverse()
        {
            return _memoryDbContext.WeatherForecasts.ToList().AsEnumerable().Reverse().ToList();
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
            var dateTicks = new DateTime(year, month, day,
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

        [HttpDelete(Name = "DeleteForecast/{id}")]
        public async Task<ActionResult<WeatherForecast>> DeleteById(long id)
        {
            var del = await _memoryDbContext.WeatherForecasts.FindAsync(id);
            if (del == null) { return NotFound(); }

            _memoryDbContext.WeatherForecasts.Remove(del);
            await _memoryDbContext.SaveChangesAsync();
            return NoContent();
        }

       
    }
}
