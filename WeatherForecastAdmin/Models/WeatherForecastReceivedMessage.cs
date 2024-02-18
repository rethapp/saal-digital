namespace WeatherForecastAdmin.Models
{

    //EXPL: this is the message sent / received <-> RabbitMq container
    public class WeatherForecastReceivedMessage
    {

        public DateTime Date { get; set; }
        public int TemperatureInCelcius { get; set; }
        public string? Summary { get; set; }
    }
}
