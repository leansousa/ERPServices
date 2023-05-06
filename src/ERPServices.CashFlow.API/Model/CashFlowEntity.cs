using ERPServices.CashFlow.API.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPServices.CashFlow.API.Model
{
    [Table("cash_flow")]
    public class CashFlowEntity : BaseEntity
    {
        [Column("description")]
        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [Column("value")]
        [Required]        
        public decimal Value { get; set; }

        [Column("type")]
        [Required]        
        [StringLength(1)]
        [MinLength(1)]
        public string? Type { get; set; }

        [Column("date_inc")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
    }
}
