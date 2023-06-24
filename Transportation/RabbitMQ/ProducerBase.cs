using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Transportation.Entities;
using Transportation.RabbitMQ.Interfaces;

namespace Transportation.RabbitMQ;

public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
{
    private readonly ILogger<ProducerBase<T>> _logger;
    public abstract string ExchangeName { get; }
    public abstract string RoutingKeyName { get; }
    public abstract string AppId { get; }

    protected ProducerBase(
        ConnectionFactory connectionFactory,
        ILogger<RabbitMqClientBase> logger,
        ILogger<ProducerBase<T>> producerBaseLogger,
        RabbitSettings rabbitSettings) :
        base(connectionFactory, logger, rabbitSettings) => _logger = producerBaseLogger;

    public virtual void Publish(T @event)
    {
        try
        {

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            var properties = Channel.CreateBasicProperties();

            properties.AppId = AppId;
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1;
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            Channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body, basicProperties: properties);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while publishing");
        }
    }
}
