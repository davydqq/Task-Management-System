using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Common.DTO;
using Transportation.RabbitMQ;
using Transportation.RabbitMQ.Impl;
using DatabaseContext.GenericRepositories;
using Common.Database;
using Microsoft.Extensions.DependencyInjection;
using Transportation.Entities;

namespace TaskManagement.Impl;

public class TaskUpdateConsumer : ConsumerBase<TaskUpdate>
{
    private readonly TaskUpdatedProducer taskUpdatedProducer;
    private readonly IServiceProvider serviceProvider;

    protected override string QueueName => "task.update";

    public TaskUpdateConsumer(
    ConnectionFactory connectionFactory,
    TaskUpdatedProducer taskUpdatedProducer,
    IServiceProvider serviceProvider,
    ILogger<ConsumerBase<TaskUpdate>> consumerLogger,
    ILogger<RabbitMqClientBase> logger,
    RabbitSettings rabbitSettings) :
    base(connectionFactory, consumerLogger, logger, rabbitSettings)
    {
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
}
