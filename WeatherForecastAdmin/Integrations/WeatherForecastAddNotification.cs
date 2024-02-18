using RabbitMQ.Client;
using System.Text.Json;
using WeatherForecastAdmin.Models;

namespace WeatherForecastAdmin.Integrations
{

    //EXPL: this class actually implements the concept of event driven architecture, as the event relative to the 
    //creation of a new weatherforecast info is published to the queue to rabbitmq.
    //I had to improve the original code to manage the reconnection and a kind of retry logic
    //A better retry logic could be implemented using external libraries (like Polly)
    //The communication with rabbitmq could be improved using more agnostic libraries like Wolverine
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

            //EXPL: the next two lines manage the reconnection
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            try
            {
                _connection = factory.CreateConnection();
            }
            catch
            {
                for (var i = 0; i < 5; i++)
                {
                    if (_connection != null)
                        continue;
                    Thread.Sleep(3000);
                    try { _connection = factory.CreateConnection(); } catch { }
                }
                if (_connection == null) throw;
            }

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _logger = logger;
        }

        public bool WeatherForecastAdded(DateTime date, int temperatureInC, string summary, int attempts = 0)
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
                return true;
            } 
            catch
            {
                Thread.Sleep(5000);
                attempts++;
                if (attempts < 5)
                {
                    return WeatherForecastAdded(date, temperatureInC, summary, attempts);                    
                } 
                else
                {
                    _logger.LogWarning("Not possible to send the message to rabbitmq :-(");
                    return false;
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
