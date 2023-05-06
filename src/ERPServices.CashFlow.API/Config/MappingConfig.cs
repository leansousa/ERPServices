using AutoMapper;
using ERPServices.CashFlow.API.Data.ValueObjects;
using ERPServices.CashFlow.API.Model;

namespace ERPServices.CashFlow.API.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CashFlowVO, CashFlowEntity>();
                config.CreateMap<CashFlowEntity, CashFlowVO>();
            });
            return mappingConfig;
        }
    }
}
