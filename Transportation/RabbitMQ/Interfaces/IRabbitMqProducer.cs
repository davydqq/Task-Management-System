namespace Transportation.RabbitMQ.Interfaces;

public interface IRabbitMqProducer<in T>
{
    void Publish(T @event);
}
