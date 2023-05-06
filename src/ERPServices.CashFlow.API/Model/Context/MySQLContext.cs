using Microsoft.EntityFrameworkCore;

namespace ERPServices.CashFlow.API.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<CashFlowEntity> CashFlows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlowEntity>().HasData(new CashFlowEntity
            {
                Id = 3,
                Description = $"Lancamento Folha de Pagamento",
                Date = new DateTime(2022,01,01),
                Type = "C",
                Value = 10
            });

            modelBuilder.Entity<CashFlowEntity>().HasData(new CashFlowEntity
            {
                Id = 4,
                Description = $"Lancamento Recebimento cliente",
                Date = new DateTime(2022, 01, 01),
                Type = "D",
                Value = 100
            });

            modelBuilder.Entity<CashFlowEntity>().HasData(new CashFlowEntity
            {
                Id = 2,
                Description = $"Lancamento Recebimento cliente 2",
                Date = new DateTime(2022, 01, 01),
                Type = "D",
                Value = 100
            });

            modelBuilder.Entity<CashFlowEntity>().HasData(new CashFlowEntity
            {
                Id = 1,
                Description = $"Lancamento Recebimento cliente 3",
                Date = new DateTime(2022, 01, 02),
                Type = "D",
                Value = 100
            });
        }
    }
}
