namespace WeatherForecastAdmin.Integrations
{
    public interface IWeatherForecastAddNotification
    {
        void WeatherForecastAdded(DateTime date, int TemperatureInC, string summary, int attempts);
    }
}
