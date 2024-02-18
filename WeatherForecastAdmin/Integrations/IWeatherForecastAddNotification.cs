namespace WeatherForecastAdmin.Integrations
{
    //EXPL: as we implement an event driven pattern, this interface is the pre-requisite to implement the 
    //class that actually will implement the signature method. In the originary code template the method was declared as void
    //and had no parameter attempts: because of the instability found about the rabbitmq container, when running in poorly
    //memory equipped Linux instances ( AWS t3.micro, free tier ), I had to manage the case when the rabbitmq queuer container
    //was temporarily unavailable.

    //EXPL: also the interface is necessary to register the service (as scoped) in the startup class ( DI pattern ) 
    public interface IWeatherForecastAddNotification
    {
        bool WeatherForecastAdded(DateTime date, int TemperatureInC, string summary, int attempts);
    }
}
