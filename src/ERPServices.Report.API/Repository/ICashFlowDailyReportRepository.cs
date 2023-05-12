using ERPServices.ReportCashFlow.API.Data.ValueObjects;

namespace ERPServices.ReportCashFlow.API.Repository
{
    public interface ICashFlowDailyReportRepository
    {
        Task<IEnumerable<CashFlowDailyReportVO>> FindAll();
        Task<IEnumerable<CashFlowDailyReportVO>> FindByPeriod(DateTime beginDate, DateTime endDate);
        Task<CashFlowDailyReportVO> FindByDate(DateTime date);

    }
}
