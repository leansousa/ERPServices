using ERPServices.ProcessCashFlow.API.Data.ValueObjects;
using ERPServices.ProcessCashFlow.API.Model;
using ERPServices.ProcessCashFlow.API.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ERPServices.Report.API.RabbitMQConsumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly CashFlowDailyProcessRepository _repository;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQMessageConsumer(CashFlowDailyProcessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            var factory = new ConnectionFactory
            {
                HostName = "10.10.0.30",
                UserName = "guest",
                Password = "guest",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "cashflowqueue", false, false, false, arguments: null);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                CashFlowMessageVO message = JsonSerializer.Deserialize<CashFlowMessageVO>(content) ?? new CashFlowMessageVO();

                ProcessItem(message).GetAwaiter().GetResult();

                _channel.BasicAck(evt.DeliveryTag, false);

            };
            _channel.BasicConsume("cashflowqueue", false, consumer);
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

                if (message.Operation == "C" || message.Operation == "D")
                {
                    if (message.Type == "C")
                        entity.TotalCredit = message.Value;
                    else if (message.Type == "D")
                        entity.TotalDebit = message.Value;
                }

                if (message.Operation == "C")
                {
                    itemDay.TotalDebit += entity.TotalDebit;
                    itemDay.TotalCredit += entity.TotalCredit;
                }
                else if (message.Operation == "D")
                {
                    itemDay.TotalDebit -= entity.TotalDebit;
                    itemDay.TotalCredit -= entity.TotalCredit;
                }
                else if (message.Operation == "U")
                {
                    if (message.Type == "C")
                        itemDay.TotalCredit = itemDay.TotalCredit + message.Value - message.ValueOld;
                    else if (message.Type == "D")
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
