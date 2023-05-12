using ERPServices.CashFlow.API.Data.ValueObjects;
using ERPServices.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ERPServices.CashFlow.API.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private readonly string _exchange;
        private readonly string _queueName;

        private readonly string _exchangeRetry;
        private readonly string _queueNameRetry;


        public RabbitMQMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _hostName = _configuration.GetValue<string>("RabbitMQ:HostName") ?? "";
            _password = _configuration.GetValue<string>("RabbitMQ:Password") ?? "";
            _userName = _configuration.GetValue<string>("RabbitMQ:UserName") ?? "";
            _exchange = _configuration.GetValue<string>("RabbitMQ:Exchange") ?? "";
            _queueName = _configuration.GetValue<string>("RabbitMQ:QueueName") ?? "";
            _exchangeRetry = $"{_exchange}_retry";
            _queueNameRetry = $"{_queueName}_retry";
        }

        public void SendMessage(BaseMessage message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
            };

            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();


            channel.ExchangeDeclare(_exchange, ExchangeType.Direct, durable: true);

            var _queueNameProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchangeRetry},
                        {"x-dead-letter-routing-key", _queueNameRetry}
                    };


            channel.QueueDeclare(_queueName, true, false, false, _queueNameProps);

            channel.QueueBind(queue: _queueName,
                              exchange: _exchange,
                              routingKey: _queueName);


            channel.ExchangeDeclare(_exchangeRetry, ExchangeType.Direct, durable: true);


            var _queueNameRetrysProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchange},
                        {"x-dead-letter-routing-key", _queueName},
                        {"x-message-ttl", 600000}
                    };

            channel.QueueDeclare(_queueNameRetry, true, false, false, _queueNameRetrysProps);

            channel.QueueBind(queue: _queueNameRetry,
                              exchange: _exchangeRetry,
                              routingKey: _queueNameRetry);


            byte[] body = GetMessageAsByteArray(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: _exchange, routingKey: _queueName, basicProperties: properties, body: body);
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize<CashFlowMessageVO>((CashFlowMessageVO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

    }
}
