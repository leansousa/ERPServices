using AutoMapper;
using ERPServices.ReportCashFlow.API.Data.ValueObjects;
using ERPServices.ReportCashFlow.API.Model;
using ERPServices.ReportCashFlow.API.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace ERPServices.ReportCashFlow.API.Repository
{
    public class CashFlowDailyReportRepository : ICashFlowDailyReportRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public CashFlowDailyReportRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CashFlowDailyReportVO>> FindAll()
        {
            List<CashFlowDailyReportEntity> cashFlows = await _context.CashFlows.ToListAsync();
            return _mapper.Map<List<CashFlowDailyReportVO>>(cashFlows);
        }

        public async Task<IEnumerable<CashFlowDailyReportVO>> FindByPeriod(DateTime beginDate, DateTime endDate)
        {
            List<CashFlowDailyReportEntity> cashFlows = await _context.CashFlows.Where(x => x.Date >= beginDate && x.Date <= endDate).ToListAsync();
            return _mapper.Map<List<CashFlowDailyReportVO>>(cashFlows);
        }

        public async Task<CashFlowDailyReportVO> FindByDate(DateTime date)
        {
            CashFlowDailyReportEntity cashFlow = await _context.CashFlows.Where(x => x.Date.Date == date.Date).FirstOrDefaultAsync() ?? new CashFlowDailyReportEntity();
            return _mapper.Map<CashFlowDailyReportVO>(cashFlow);
        }

    }
}
