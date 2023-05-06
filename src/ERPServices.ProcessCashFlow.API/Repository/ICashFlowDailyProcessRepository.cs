using ERPServices.ProcessCashFlow.API.Model;

namespace ERPServices.ProcessCashFlow.API.Repository
{
    public interface ICashFlowDailyProcessRepository
    {
        Task<CashFlowDailyReportEntity> FindByDate(DateTime date);

        Task<CashFlowDailyReportEntity> Create(CashFlowDailyReportEntity entity);
        Task<CashFlowDailyReportEntity> Update(CashFlowDailyReportEntity entity);
    }
}
