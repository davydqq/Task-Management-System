using TaskManagement.Interfaces;

namespace API.Hosted;

public class SetupServiceBusHosted : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public SetupServiceBusHosted(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var serviceBusHandler = scope.ServiceProvider.GetRequiredService<IServiceBusHandler>();

        serviceBusHandler.ReceiveMessage();

        return Task.CompletedTask;
    }
}
