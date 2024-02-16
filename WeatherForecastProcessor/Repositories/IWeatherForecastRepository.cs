using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastProcessor.Repositories
{
    public interface IWeatherForecastRepository
    {
        Task SaveWeatherForecastInformation(DateTime date,
                                            int temperatureInC,
                                            string summary);
    }
}
