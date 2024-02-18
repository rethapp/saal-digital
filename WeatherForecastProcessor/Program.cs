using WeatherForecastProcessor.Repositories;

namespace WeatherForecastProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //EXPL: DI pre-requisite to inject the repository into worker.
                    services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();
                    services.AddHostedService<Worker>();
                });
    }
}