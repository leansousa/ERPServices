using ERPServices.MessageBus;

namespace ERPServices.CashFlow.API.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage);
    }
}
