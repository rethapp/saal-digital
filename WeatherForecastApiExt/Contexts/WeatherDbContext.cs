using Microsoft.EntityFrameworkCore;
using WeatherForecastApiExt.Models;

namespace WeatherForecastApiExt.Contexts
{

    //EXPL: EFCore basic implementation to communicate with the postgresql database
    //this class is also used to create and initialize the database weatherdb, using these commands from the .sln dir:
    //dotnet ef migrations add InitialMigration
    //dotnet ef database update
    //these commands create the special file located under the Migrations directory
    // to actually "migrate" the database, this code 
    /*
     using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var context = serviceScope.ServiceProvider.GetRequiredService<WeatherDbContext>();			
			context.Database.Migrate();
     */
    //in the Configure method in the Startup class
    public class WeatherDbContext : DbContext
    {

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {

        }

        //EXPL: the next lines, together with LINQ instruction set allow us to perform all the operations we need --> postgresql
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        //EXPL: thanks to the next lines of code, we can initialize our table with some records...
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var random = new Random();
            modelBuilder.Entity<WeatherForecast>().HasData(
                new WeatherForecast() { Id = 1, Date = DateTime.Now.AddDays(1).ToUniversalTime(), Summary = "Freezing", TemperatureC = random.Next(-20, 55) },
                new WeatherForecast() { Id = 2, Date = DateTime.Now.AddDays(2).ToUniversalTime(), Summary = "Bracing", TemperatureC = random.Next(-20, 55) },
                new WeatherForecast() { Id = 3, Date = DateTime.Now.AddDays(3).ToUniversalTime(), Summary = "Chilly", TemperatureC = random.Next(-20, 55) }

                );
        }

    }
}
