using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherForecastAdmin.Integrations;

namespace WeatherForecastAdmin.Pages
{

    [BindProperties]
    public class WeatherForecastModel : PageModel
    {
        private readonly ILogger<WeatherForecastModel> _logger;
        private readonly IWeatherForecastAddNotification _weatherForecastAddNotification;

        public DateTime Date { get; set; }
        public int TemperatureInCelcius { get; set; }
        public string? Summary { get; set; }

        public WeatherForecastModel(ILogger<WeatherForecastModel> logger,
                                    IWeatherForecastAddNotification weatherForecastAddNotification)
        {
            _logger = logger;
            _weatherForecastAddNotification = weatherForecastAddNotification;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            _weatherForecastAddNotification.WeatherForecastAdded(Date, TemperatureInCelcius, Summary!, 0);
        }
    }
}
