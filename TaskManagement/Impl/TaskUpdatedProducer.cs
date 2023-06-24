using Common.DTO;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Transportation.Entities;

namespace Transportation.RabbitMQ.Impl;

public class TaskUpdatedProducer : ProducerBase<TaskUpdate>
{
    public override string ExchangeName => "task";

    public override string RoutingKeyName => "updated";

    public override string AppId => "default";

    public TaskUpdatedProducer(
        ConnectionFactory connectionFactory, 
        ILogger<RabbitMqClientBase> logger, 
        ILogger<ProducerBase<TaskUpdate>> producerBaseLogger,
        RabbitSettings rabbitSettings) : 
        base(connectionFactory, logger, producerBaseLogger, rabbitSettings)
    {
    }
}
