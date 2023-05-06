namespace ERPServices.ReportCashFlow.API.Data.ValueObjects
{
    public class CashFlowDailyReportVO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal Total { get { return TotalDebit - TotalCredit; } }
    }
}
