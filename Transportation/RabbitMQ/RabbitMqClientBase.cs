using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Transportation.Entities;

namespace Transportation.RabbitMQ;

public abstract class RabbitMqClientBase : IDisposable
{
    protected IModel Channel { get; private set; }

    private IConnection _connection;

    private readonly ConnectionFactory _connectionFactory;

    private readonly ILogger<RabbitMqClientBase> _logger;

    private readonly RabbitSettings rabbitSettings;

    protected RabbitMqClientBase(
        ConnectionFactory connectionFactory,
        ILogger<RabbitMqClientBase> logger,
        RabbitSettings rabbitSettings)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        this.rabbitSettings = rabbitSettings;
        ConnectToRabbitMq();
    }

    private void ConnectToRabbitMq()
    {
        if (_connection == null || _connection.IsOpen == false)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        if (Channel == null || Channel.IsOpen == false)
        {
            Channel = _connection.CreateModel();

            foreach(var exchange in rabbitSettings.Exchanges)
            {
                Channel.ExchangeDeclare(
                    exchange: exchange.Exchange,
                    type: exchange.Type,
                    durable: exchange.Durable,
                    autoDelete: exchange.AutoDelete);
            }

            foreach (var setting in rabbitSettings.QueueDeclarations)
            {
                Channel.QueueDeclare(
                    queue: setting.Queue,
                    durable: setting.Durable,
                    exclusive: setting.Exclusive,
                    autoDelete: setting.AutoDelete);
            }

            foreach (var setting in rabbitSettings.QueueBinds)
            {
                Channel.QueueBind(
                    queue: setting.Queue,
                    exchange: setting.Exchange,
                    routingKey: setting.RoutingKey);
            }
        }
    }

    public void Dispose()
    {
        try
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
        }
    }
}
