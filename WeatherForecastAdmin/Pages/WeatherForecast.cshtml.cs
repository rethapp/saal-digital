using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WeatherForecastAdmin.CustomAttributes;
using WeatherForecastAdmin.Integrations;

namespace WeatherForecastAdmin.Pages
{

    //EXPL: this is the background logic to the main UI, a Razor page that allows to enter new weather forecast info
    //I added some validation, using ValidationAttributes...
    //The validation and the information for the user has been improved also.
    [BindProperties]
    public class WeatherForecastModel : PageModel
    {
        private readonly ILogger<WeatherForecastModel> _logger;
        private readonly IWeatherForecastAddNotification _weatherForecastAddNotification;

        [Required(ErrorMessage = "Please, enter a valid date")]
        [DateRangeValidationAttribute]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage="The temperature must be in the range -60 +60 degrees in Celsius")]
        [Range(-60, 60)]
        public int TemperatureInCelcius { get; set; } = 0;

        [Required(ErrorMessage = "Please, describe the weather")]
        public string? Summary { get; set; } = String.Empty;

        //EXPL: dependency injection pattern detected, possible because of AddScoped....
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
            if (ModelState.IsValid)
            {

                bool b = _weatherForecastAddNotification.WeatherForecastAdded(Date, TemperatureInCelcius, Summary!, 0);
                ViewData["ResultOk"] = b == true ? "Message sent to the queue" : "";
                ViewData["ResultKo"] = b == true ? "" : "Message NOT sent to the queue!";
                
                //reste the fields
                Date = DateTime.Now;
                TemperatureInCelcius = 0;
                Summary = String.Empty;

            }             
            
        }
    }
}
