using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Common.DTO;
using Transportation.RabbitMQ;
using Transportation.RabbitMQ.Impl;
using DatabaseContext.GenericRepositories;
using Common.Database;
using Microsoft.Extensions.DependencyInjection;
using Transportation.Entities;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;

namespace TaskManagement.Impl;

public class TaskUpdateConsumer : ConsumerBase<TaskUpdate>
{
    private readonly TaskUpdateProducer taskUpdateProducer;
    private readonly TaskUpdatedProducer taskUpdatedProducer;
    private readonly IServiceProvider serviceProvider;

    protected override string QueueName => "task.update";

    public TaskUpdateConsumer(
    ConnectionFactory connectionFactory,
    TaskUpdateProducer taskUpdateProducer,
    TaskUpdatedProducer taskUpdatedProducer,
    IServiceProvider serviceProvider,
    ILogger<ConsumerBase<TaskUpdate>> consumerLogger,
    ILogger<RabbitMqClientBase> logger,
    RabbitSettings rabbitSettings) :
    base(connectionFactory, consumerLogger, logger, rabbitSettings)
    {
        this.taskUpdateProducer = taskUpdateProducer;
        this.taskUpdatedProducer = taskUpdatedProducer;
        this.serviceProvider = serviceProvider;
    }

    public override async Task ProcessMessageAsync(TaskUpdate message)
    {
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TaskEntity, int>>();

        var task = await repository.FirstOrDefaultAsync(x => x.TaskID == message.TaskID);

        if(task != null)
        {
            task.StatusId = message.NewStatus;
            await repository.UpdateAsync(task);

            taskUpdatedProducer.Publish(message);
        }
    }

    protected override async Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var body = Encoding.UTF8.GetString(@event.Body.ToArray());
            var message = JsonSerializer.Deserialize<TaskUpdate>(body);

            await ProcessMessageAsync(message);
            Channel.BasicAck(@event.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while retrieving message from queue.");
            RetryLogic(@event);
        }
    }

    private void RetryLogic(BasicDeliverEventArgs @event)
    {
        // Handle the exception and decide whether to retry or discard the message
        var retryAttempts = 2; // Number of additional retry attempts

        var currentRetryCount = 0;
        if (@event.BasicProperties.Headers != null && @event.BasicProperties.Headers.TryGetValue("RetryCount", out var count))
        {
            currentRetryCount = (int)count;
        }

        if (currentRetryCount < retryAttempts)
        {
            // Increment the retry count and publish the message back to the queue for retry
            var newRetryCount = currentRetryCount + 1;
            var properties = Channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
                {
                { "RetryCount", newRetryCount }
            };

            Channel.BasicAck(@event.DeliveryTag, false);
            Channel.BasicPublish(taskUpdateProducer.ExchangeName, taskUpdateProducer.RoutingKeyName, properties, @event.Body.ToArray());
            Console.WriteLine("Message requeued for retry. Retry count: " + newRetryCount);
        }
        else
        {
            // Retry attempts exceeded, discard the message
            Channel.BasicNack(@event.DeliveryTag, false, false);
            Console.WriteLine("Message discarded. Retry attempts exceeded.");
        }
    }
}
