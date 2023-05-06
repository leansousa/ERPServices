using ERPServices.MessageBus;

namespace ERPServices.ProcessCashFlow.API.Data.ValueObjects
{
    public class CashFlowMessageVO :BaseMessage
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public string? Type { get; set; }
        public DateTime Date { get; set; }
        public string? Operation { get; set; }
        public decimal ValueOld { get; set; }
    }
}
