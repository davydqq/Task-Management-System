using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Transportation;

public static class TransportationModules
{
    public static void ApplyTransportationModules(this IServiceCollection services, string rabbitMQConnection)
    {
        services
            .AddSingleton(serviceProvider =>
            {
                var uri = new Uri(rabbitMQConnection);
                return new ConnectionFactory
                {
                    Uri = uri,
                    DispatchConsumersAsync = true,
                };
            });
    }
}
