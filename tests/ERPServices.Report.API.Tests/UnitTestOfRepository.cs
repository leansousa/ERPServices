using AutoMapper;
using ERPServices.ReportCashFlow.API.Config;
using ERPServices.ReportCashFlow.API.Data.ValueObjects;
using ERPServices.ReportCashFlow.API.Model.Context;
using ERPServices.ReportCashFlow.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ERPServices.Report.API.Tests
{
    [TestCaseOrderer(ordererTypeName: "ERPServices.Report.API.Tests.OrderHelper", ordererAssemblyName: "ERPServices.Report.API.Tests")]
    public class UnitTestOfRepository
    {

        private readonly DbContextOptionsBuilder<MySQLContext> _dbContextOptions;
        private readonly ICashFlowDailyReportRepository _repository;
        private static IMapper _mapper;
        private readonly IConfiguration _configuration;
        private static long idLancamento;
        public UnitTestOfRepository()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();

            var connectionString = _configuration.GetValue<string>("MySqlConnection:MysqlConnectionString") ?? "";

            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            if (_mapper == null)
            {
                IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
                _mapper = mapper;
            }

            if (_repository == null)
            {
                MySQLContext context = new(_dbContextOptions.Options);
                _repository = new CashFlowDailyReportRepository(context, _mapper);
            }


        }

       
        [Fact]
        public async Task CTU_001_BuscarTodosOsRegistrosParaRelatorio()
        {

            var result = await _repository.FindAll();

            Assert.True(result.Any());
        }

        [Fact]
        public async Task CTU_002_BuscarPorPeriodoOsRegistrosParaRelatorio()
        {

            var result = await _repository.FindByPeriod(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(+1));

            Assert.True(result.Any());
        }

        [Fact]
        public async Task CTU_003_BuscarPorDataATualOsRegistrosParaRelatorio()
        {

            var result = await _repository.FindByDate(DateTime.Now);

            Assert.True(result != null);
        }


        
        
    }
}