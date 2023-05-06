using Microsoft.EntityFrameworkCore;

namespace ERPServices.ReportCashFlow.API.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<CashFlowDailyReportEntity> CashFlows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlowDailyReportEntity>().HasData(new CashFlowDailyReportEntity
            {
                Id = 1,
                Date = new DateTime(2022, 01, 01),
                TotalCredit = 100,
                TotalDebit = 200,
            });

            modelBuilder.Entity<CashFlowDailyReportEntity>().HasData(new CashFlowDailyReportEntity
            {
                Id = 2,
                Date = new DateTime(2022, 01, 02),
                TotalCredit = 140,
                TotalDebit = 210,
            });

            modelBuilder.Entity<CashFlowDailyReportEntity>().HasData(new CashFlowDailyReportEntity
            {
                Id = 3,
                Date = new DateTime(2022, 01, 03),
                TotalCredit = 1000,
                TotalDebit = 2000,
            });

            modelBuilder.Entity<CashFlowDailyReportEntity>().HasData(new CashFlowDailyReportEntity
            {
                Id = 4,
                Date = new DateTime(2022, 01, 04),
                TotalCredit = 160,
                TotalDebit = 0,
            });
        }
    }
}
