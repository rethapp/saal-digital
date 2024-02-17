using RabbitMQ.Client;
using System.Text.Json;
using WeatherForecastAdmin.Models;

namespace WeatherForecastAdmin.Integrations
{
    public class WeatherForecastAddNotification : IWeatherForecastAddNotification, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string queueName = "weatherforecast.received";
        private readonly ILogger<WeatherForecastAddNotification> _logger;
        

        public WeatherForecastAddNotification(IConfiguration config,
                                              ILogger<WeatherForecastAddNotification> logger)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "tony";
            factory.Password = "koszka80";
            factory.VirtualHost = "/";            
            factory.HostName = config.GetValue<string>("RabbitMqHost");
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _logger = logger;
        }

        public void WeatherForecastAdded(DateTime date, int temperatureInC, string summary, int attempts = 0)
        {
            var message = new WeatherForecastReceivedMessage
            {
                Date = date,
                TemperatureInCelcius = temperatureInC,
                Summary = summary
            };

            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message);
            try
            {
                _channel.BasicPublish("", routingKey: queueName, basicProperties: null, body: messageBytes);
                _logger.LogInformation($"Published weather forecast notification. " +
                                       $"Date: {date} " +
                                       $"Temperature in Celsius: {temperatureInC} " +
                                       $"Summary: {summary}");
            } 
            catch (Exception ex)
            {
                Thread.Sleep(5000);
                attempts++;
                if (attempts < 5)
                {
                    WeatherForecastAdded(date, temperatureInC, summary, attempts);
                }
            }
            
        }

        

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
