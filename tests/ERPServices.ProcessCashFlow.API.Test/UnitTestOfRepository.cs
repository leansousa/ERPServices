using ERPServices.ProcessCashFlow.API.Model;
using ERPServices.ProcessCashFlow.API.Model.Context;
using ERPServices.ProcessCashFlow.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ERPServices.Report.API.Tests
{
    [TestCaseOrderer(ordererTypeName: "ERPServices.ProcessCashFlow.API.Tests.OrderHelper", ordererAssemblyName: "ERPServices.ProcessCashFlow.API.Tests")]
    public class UnitTestOfRepository
    {

        private readonly DbContextOptionsBuilder<MySQLContext> _dbContextOptions;
        private readonly ICashFlowDailyProcessRepository _repository;        
        private readonly IConfiguration _configuration;
        private static long idLancamento;
        public UnitTestOfRepository()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();

            var connectionString = _configuration.GetValue<string>("MySqlConnection:MysqlConnectionString") ?? "";

            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            _repository ??= new CashFlowDailyProcessRepository(_dbContextOptions.Options);


        }


        [Fact]
        public async Task CTU_001_CriarRegistroDaDataAtualParaRelatorio()
        {

            var vo = new CashFlowDailyReportEntity()
            {
                Date = DateTime.Now,
                TotalCredit = 100,
                TotalDebit = 100,
            };

            var result = await _repository.Create(vo);

            idLancamento = result.Id;

            Assert.True(result.Id > 0);
        }



        [Fact]
        public async Task CTU_002_AlterarRegistroDaDataAtualParaRelatorio()
        {
            var vo = new CashFlowDailyReportEntity
            {
                Id = idLancamento,
                Date = DateTime.Now,
                TotalCredit = 200,
                TotalDebit = 200,
            };

            var result = await _repository.Update(vo);

            var alterado = await _repository.FindByDate(vo.Date);

            Assert.True(alterado.TotalCredit == vo.TotalCredit && alterado.TotalDebit == vo.TotalDebit);
        }

        
        [Fact]
        public async Task CTU_003_BuscarPorDataATualOsRegistrosParaRelatorio()
        {

            var result = await _repository.FindByDate(DateTime.Now);

            Assert.True(result != null);
        }


        
        
    }
}