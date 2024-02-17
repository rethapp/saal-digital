namespace WeatherForecastAdmin.Integrations
{
    public interface IWeatherForecastAddNotification
    {
        bool WeatherForecastAdded(DateTime date, int TemperatureInC, string summary, int attempts);
    }
}
