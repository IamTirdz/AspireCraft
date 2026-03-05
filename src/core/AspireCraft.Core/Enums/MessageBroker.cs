using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum MessageBroker
{
    [Display(Name = "None")]
    None,

    [Display(Name = "RabbitMQ")]
    RabbitMQ,

    [Display(Name = "Azure Service Bus")]
    ServiceBus,

    [Display(Name = "Kafka")]
    Kafka
}
