using AutoMapper;
using ERPServices.ReportCashFlow.API.Data.ValueObjects;
using ERPServices.ReportCashFlow.API.Model;

namespace ERPServices.ReportCashFlow.API.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CashFlowDailyReportVO, CashFlowDailyReportEntity>();
                config.CreateMap<CashFlowDailyReportEntity, CashFlowDailyReportVO>();
            });
            return mappingConfig;
        }
    }
}
