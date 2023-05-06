using ERPServices.MessageBus;

namespace ERPServices.CashFlow.API.Data.ValueObjects
{
    public class CashFlowVO 
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public string? Type { get; set; }
        public DateTime Date { get; set; }
    }
}
