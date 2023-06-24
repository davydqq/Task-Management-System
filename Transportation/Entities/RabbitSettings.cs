

namespace Transportation.Entities;

public class RabbitSettings
{
    public List<ExchangeSettings> Exchanges { get; set; }
    public List<QueueDeclarationSettings> QueueDeclarations { get; set; }
    public List<QueueBindSettings> QueueBinds { get; set; }
}
