using ERPServices.ProcessCashFlow.API.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPServices.ProcessCashFlow.API.Model
{
    [Table("cash_flow_daily_report")]
    public class CashFlowDailyReportEntity : BaseEntity
    {
        [Column("date_inc")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [Column("total_debit")]
        [Required]
        public decimal TotalDebit { get; set; }

        [Column("total_credit")]
        [Required]
        public decimal TotalCredit { get; set; }

    }
}
