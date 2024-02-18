using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastProcessor.Repositories
{

    //EXPL: class implements the interface... necessary to DI..
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

        //EXPL: here we do not use EF but the Npgsql package to open the connection to postgres and perform the insert
        //I had to fix two bugs that were present in the original code: one relative to the datetime type and one about
        //using the parameters with command instead of direct sql query!!!
        public async Task SaveWeatherForecastInformation(DateTime date,
                                                         int temperatureInC,
                                                         string summary)
        {

            int result = 0;
            
            DateTime dt = new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, 
                                        DateTime.Now.Second, DateTime.Now.Millisecond, DateTimeKind.Utc).AddTicks(4414);

            //EXPL: var is used as it is inferred by the compiler at compile time.
            using var connection = new NpgsqlConnection(_config.GetConnectionString("WeatherDatabase"));
            connection.Open();
            
            string dml = $"INSERT INTO public.\"WeatherForecasts\" (\"Date\", \"TemperatureC\", \"Summary\") VALUES (:dt, :temp, :summary)";

            //EXPL: always good to use 'using' in order to immediately dispose the object cmd. Same for the connection obj
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
