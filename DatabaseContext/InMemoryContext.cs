using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.DatabaseContext;

public class InMemoryContext : DbContext
{
    public InMemoryContext(DbContextOptions<InMemoryContext> options)
        : base(options)
    {

    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}