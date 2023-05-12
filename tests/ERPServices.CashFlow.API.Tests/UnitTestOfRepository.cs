using AutoMapper;
using ERPServices.CashFlow.API.Config;
using ERPServices.CashFlow.API.Data.ValueObjects;
using ERPServices.CashFlow.API.Model.Context;
using ERPServices.CashFlow.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ERPServices.CashFlow.API.Tests
{
    [TestCaseOrderer(ordererTypeName: "ERPServices.CashFlow.API.Tests.OrderHelper", ordererAssemblyName: "ERPServices.CashFlow.API.Tests")]
    public class UnitTestOfRepository
    {

        private readonly DbContextOptionsBuilder<MySQLContext> _dbContextOptions;
        private readonly ICashFlowRepository _repository;
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
                _repository = new CashFlowRepository(context, _mapper);
            }


        }

        [Fact]
        public async Task CTU_001_InserirLancamentoComSucesso()
        {
            var vo = new CashFlowVO
            {
                Value = 123,
                Type = "C",
                Date = DateTime.Now,
                Description = "Test"
            };

            var result = await _repository.Create(vo);

            idLancamento = result.Id;

            Assert.True(idLancamento > 0);
        }

        [Fact]
        public async Task CTU_002_AlterarLancamentoComSucesso()
        {
            var vo = new CashFlowVO
            {
                Id = idLancamento,
                Value = 1234,
                Type = "D",
                Date = DateTime.Now,
                Description = "Test Alterado"
            };

            var result = await _repository.Update(vo);

            var alterado = await _repository.FindbyId(idLancamento);

            Assert.True(alterado.Value == vo.Value && alterado.Description == vo.Description && alterado.Type == vo.Type);
        }

        [Fact]
        public async Task CTU_003_BuscarTodosOsLancamentosComSucesso()
        {

            var result = await _repository.FindAll();

            Assert.True(result.Any());
        }

        [Fact]
        public async Task CTU_004_ExcluirLancamentoComSucesso()
        {
            
            var result = await _repository.Delete(idLancamento);

            var excluido = await _repository.FindbyId(idLancamento);
            
            Assert.True(excluido.Id <= 0 && result == true);
        }
    }
}