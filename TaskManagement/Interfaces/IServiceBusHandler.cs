using Common.DTO;

namespace TaskManagement.Interfaces;

public interface IServiceBusHandler
{
    void SendMessage(TaskUpdate taskUpdate);
    void ReceiveMessage();
}
