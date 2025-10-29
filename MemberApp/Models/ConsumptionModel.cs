using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MemberApp.Dtos; // for CalculationMethod if you defined enum there

namespace MemberApp.Models
{
    public class ConsumptionModel
    {
        [Key]
        public int ConsumptionId { get; set; }

        public int MemberId { get; set; }

        [Required]
        [StringLength(100)]
        public string MemberName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? MemberPhone { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(12,0)")]
        public decimal TotalBeforeDiscount { get; set; }

        [Column(TypeName = "decimal(12,0)")]
        public decimal TotalAfterDiscount { get; set; }

        public ICollection<ConsumptionLineModel> Lines { get; set; } = new List<ConsumptionLineModel>();
    }

    public class ConsumptionLineModel
    {
        [Key]
        public int ConsumptionLineId { get; set; }

        [ForeignKey(nameof(Consumption))]
        public int ConsumptionId { get; set; }

        public ConsumptionModel? Consumption { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,0)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        // subtotal = UnitPrice * Quantity
        [Column(TypeName = "decimal(12,0)")]
        public decimal LineSubtotal { get; set; }

        // Applied coupon (optional)
        public int? CouponId { get; set; }

        [StringLength(100)]
        public string? CouponName { get; set; }

        // store coupon raw info so history stays immutable
        //CalculationMethod is enum: 減=0, 乘=1
        //CouponModel.CalculationMethod
        public CalculationMethod? CouponMethod { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal? CouponValue { get; set; }

        // how much discount was applied (positive when discount reduces price)
        [Column(TypeName = "decimal(12,0)")]
        public decimal DiscountAmount { get; set; }

        // final line total after coupon
        [Column(TypeName = "decimal(12,0)")]
        public decimal LineTotal { get; set; }
    }
}
