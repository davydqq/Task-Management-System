using Common.DTO;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Transportation.Entities;

namespace Transportation.RabbitMQ.Impl;

public class TaskUpdateProducer : ProducerBase<TaskUpdate>
{
    public override string ExchangeName => "task";

    public override string RoutingKeyName => "update";

    public override string AppId => "default";

    public TaskUpdateProducer(
        ConnectionFactory connectionFactory, 
        ILogger<RabbitMqClientBase> logger, 
        ILogger<ProducerBase<TaskUpdate>> producerBaseLogger,
        RabbitSettings rabbitSettings) :
        base(connectionFactory, logger, producerBaseLogger, rabbitSettings)
    {
    }
}
