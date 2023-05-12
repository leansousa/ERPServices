using ERPServices.ProcessCashFlow.API.Data.ValueObjects;
using ERPServices.ProcessCashFlow.API.Model;
using ERPServices.ProcessCashFlow.API.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace ERPServices.Report.API.RabbitMQConsumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly CashFlowDailyProcessRepository _repository;

        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly string _exchange;
        private readonly string _queueName;
        private readonly string _exchangeRetry;
        private readonly string _queueNameRetry;

        public RabbitMQMessageConsumer(CashFlowDailyProcessRepository repository, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(repository));

            _exchange = _configuration.GetValue<string>("RabbitMQ:Exchange") ?? "";
            _queueName = _configuration.GetValue<string>("RabbitMQ:QueueName") ?? "";

            _exchangeRetry = $"{_exchange}_retry";
            _queueNameRetry = $"{_queueName}_retry";

            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetValue<string>("RabbitMQ:HostName") ?? "",
                UserName = _configuration.GetValue<string>("RabbitMQ:Password") ?? "",
                Password = _configuration.GetValue<string>("RabbitMQ:UserName") ?? "",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchange, ExchangeType.Direct, durable: true);

            var _queueNameProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchangeRetry},
                        {"x-dead-letter-routing-key", _queueNameRetry}
                    };


            _channel.QueueDeclare(_queueName, true, false, false, _queueNameProps);

            _channel.QueueBind(queue: _queueName,
                              exchange: _exchange,
                              routingKey: _queueName);


            _channel.ExchangeDeclare(_exchangeRetry, ExchangeType.Direct, durable: true);


            var _queueNameRetrysProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchange},
                        {"x-dead-letter-routing-key", _queueName},
                        {"x-message-ttl", 10000},
                        {"x-redelivered-count", 2},
                    };

            _channel.QueueDeclare(_queueNameRetry, true, false, false, _queueNameRetrysProps);

            _channel.QueueBind(queue: _queueNameRetry,
                              exchange: _exchangeRetry,
                              routingKey: _queueNameRetry);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                    CashFlowMessageVO message = JsonSerializer.Deserialize<CashFlowMessageVO>(content) ?? new CashFlowMessageVO();

                    ProcessItem(message).GetAwaiter().GetResult();
                }
                catch
                {
                    _channel.BasicNack(evt.DeliveryTag, false, false);
                    throw new Exception($"Error in process message from {_queueName} {evt.DeliveryTag}");
                }

                _channel.BasicAck(evt.DeliveryTag, false);

            };
            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessItem(CashFlowMessageVO message)
        {
            try
            {
                if (message == null) throw new Exception("Error in convert object - object is null");

                CashFlowDailyReportEntity entity = new()
                {
                    Date = message.Date.Date
                };

                var itemDay = await _repository.FindByDate(entity.Date);

                if (message.Operation == "C") // Criação
                {
                    if (message.Type == "D") // Débito
                        itemDay.TotalDebit += message.Value;
                    else if (message.Type == "C") //Crédito
                        itemDay.TotalCredit += message.Value;
                }
                else if (message.Operation == "D") //Exclusão
                {
                    if (message.Type == "D") // Débito
                        itemDay.TotalDebit -= message.Value;
                    else if (message.Type == "C") //Crédito
                        itemDay.TotalCredit -= message.Value;
                }
                else if (message.Operation == "U") //Atualização
                {
                    if (message.Type == "C") //Crédito
                        itemDay.TotalCredit = itemDay.TotalCredit + message.Value - message.ValueOld;
                    else if (message.Type == "D") // Débito
                        itemDay.TotalDebit = itemDay.TotalDebit + message.Value - message.ValueOld;
                }

                itemDay.Date = entity.Date;

                if (itemDay.Id > 0)
                {
                    await _repository.Update(itemDay);
                }
                else
                {
                    await _repository.Create(itemDay);
                }


            }
            catch
            {
                throw new Exception("Error in convert object");
            }
        }
    }
}
