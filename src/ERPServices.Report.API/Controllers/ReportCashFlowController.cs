using ERPServices.ReportCashFlow.API.Data.ValueObjects;
using ERPServices.ReportCashFlow.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERPServices.CashFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportCashFlowController : ControllerBase
    {
        private readonly ICashFlowDailyReportRepository _repository;
        public ReportCashFlowController(ICashFlowDailyReportRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashFlowDailyReportVO>>> FindAll()
        {
            var result = await _repository.FindAll();
            return Ok(result);
        }

        [HttpGet("{beginDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<CashFlowDailyReportVO>>> FindByPeriod(DateTime beginDate, DateTime endDate)
        {
            var result = await _repository.FindByPeriod(beginDate, endDate);
            return Ok(result);
        }


    }
}
