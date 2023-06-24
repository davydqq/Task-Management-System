using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Transportation.Entities;

namespace Transportation.RabbitMQ;

public abstract class ConsumerBase<T> : RabbitMqClientBase
{
    private readonly ILogger<ConsumerBase<T>> _logger;
    protected abstract string QueueName { get; }

    public ConsumerBase(
        ConnectionFactory connectionFactory,
        ILogger<ConsumerBase<T>> consumerLogger,
        ILogger<RabbitMqClientBase> logger,
        RabbitSettings rabbitSettings) :
        base(connectionFactory, logger, rabbitSettings)
    {
        _logger = consumerLogger;
    }

    public virtual void Init()
    {
        try
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += OnEventReceived;
            Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while consuming message");
        }
    }

    protected virtual async Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var body = Encoding.UTF8.GetString(@event.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(body);

            await ProcessMessageAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while retrieving message from queue.");
        }
        finally
        {
            Channel.BasicAck(@event.DeliveryTag, false);
        }
    }

    public abstract Task ProcessMessageAsync(T message);
}