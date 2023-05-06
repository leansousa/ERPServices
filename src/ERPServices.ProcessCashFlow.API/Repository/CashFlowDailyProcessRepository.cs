using ERPServices.ProcessCashFlow.API.Model;
using ERPServices.ProcessCashFlow.API.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace ERPServices.ProcessCashFlow.API.Repository
{
    public class CashFlowDailyProcessRepository : ICashFlowDailyProcessRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public CashFlowDailyProcessRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task<CashFlowDailyReportEntity> FindByDate(DateTime date)
        {
            await using var _db = new MySQLContext(_context);
            CashFlowDailyReportEntity cashFlow = await _db.CashFlows.Where(x => x.Date == date).FirstOrDefaultAsync() ?? new CashFlowDailyReportEntity();
            return cashFlow;
        }

        public async Task<CashFlowDailyReportEntity> Create(CashFlowDailyReportEntity cashFlow)
        {
            await using var _db = new MySQLContext(_context);
            _db.CashFlows.Add(cashFlow);
            await _db.SaveChangesAsync();
            return cashFlow;
        }

        public async Task<CashFlowDailyReportEntity> Update(CashFlowDailyReportEntity cashFlow)
        {
            await using var _db = new MySQLContext(_context);
            _db.CashFlows.Update(cashFlow);
            await _db.SaveChangesAsync();
            return cashFlow;
        }
    }
}
