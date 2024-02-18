using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecastProcessor.Models
{

    //EXPL: this must be the same as the class used into the project Admin: there could have been a refactoring, using a shared library
    class WeatherForecastReceivedMessage
    {
        public DateTime Date { get; set; }
        public int TemperatureInCelcius { get; set; }
        public string Summary { get; set; }
    }
}
