using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastProcessor.Repositories
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<WeatherForecastRepository> _logger;

        public WeatherForecastRepository(IConfiguration config,
                                         ILogger<WeatherForecastRepository> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task SaveWeatherForecastInformation(DateTime date,
                                                         int temperatureInC,
                                                         string summary)
        {

            int result = 0;
            
            DateTime dt = new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, 
                                        DateTime.Now.Second, DateTime.Now.Millisecond, DateTimeKind.Utc).AddTicks(4414);

            using var connection = new NpgsqlConnection(_config.GetConnectionString("WeatherDatabase"));
            connection.Open();
            //string dml = "insert into WeatherForecasts (Date, TemperatureC, Summary) values (:dt, :temp, :summary)";
            string dml = $"INSERT INTO public.\"WeatherForecasts\" (\"Date\", \"TemperatureC\", \"Summary\") VALUES (:dt, :temp, :summary)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(dml, connection))
            {
                cmd.Parameters.AddWithValue("dt", dt);
                cmd.Parameters.AddWithValue("temp", temperatureInC);
                cmd.Parameters.AddWithValue("summary", summary);

                try
                {
                    result = cmd.ExecuteNonQuery();
                    _logger.LogInformation($"{result.ToString()} record inserted");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

        }
    }
}
