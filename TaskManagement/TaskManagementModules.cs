using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Impl;
using TaskManagement.Interfaces;
using Transportation.RabbitMQ.Impl;

namespace TaskManagement;

public static class TaskManagementModules
{
    public static void ApplyTaskManagementModules(this IServiceCollection services)
    {
        services.AddSingleton<TaskUpdateConsumer>();
        services.AddSingleton<TaskUpdateProducer>();
        services.AddSingleton<TaskUpdatedProducer>();

        services.AddTransient<IServiceBusHandler, ServiceBusHandler>();
    }
}
