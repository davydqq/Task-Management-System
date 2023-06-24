using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Interfaces;
using Transportation.RabbitMQ.Impl;

namespace TaskManagement.Impl;

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly TaskUpdateConsumer taskUpdateConsumer;
    private readonly TaskUpdateProducer taskUpdateProducer;

    public ServiceBusHandler(TaskUpdateConsumer taskUpdateConsumer, TaskUpdateProducer taskUpdateProducer)
    {
        this.taskUpdateConsumer = taskUpdateConsumer;
        this.taskUpdateProducer = taskUpdateProducer;
    }


    public void SendMessage(TaskUpdate taskUpdate)
    {
        taskUpdateProducer.Publish(taskUpdate);
    }

    public void ReceiveMessage()
    {
        taskUpdateConsumer.Init();
    }
}
