using System.ComponentModel.DataAnnotations;
using MemberApp.Models; // for CalculationMethod enum

namespace MemberApp.Dtos
{
    public class ConsumptionLineDto
    {
        public int ConsumptionLineId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        [Range(1, 10000)]
        public int Quantity { get; set; } = 1;

        public int? CouponId { get; set; }

        public string? CouponName { get; set; }

        public CalculationMethod? CouponMethod { get; set; }

        // The raw coupon number (e.g. 0.8 for multiply 20% off, or 500 for subtract)
        public decimal? CouponValue { get; set; }

        // Calculated
        public decimal LineSubtotal { get; set; }

        public decimal LineTotal { get; set; }

        public decimal DiscountAmount { get; set; }
    }

    public class ConsumptionDto
    {
        public int ConsumptionId { get; set; }
        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string MemberPhone { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public List<ConsumptionLineDto> Lines { get; set; } = new List<ConsumptionLineDto>();

        public decimal TotalBeforeDiscount { get; set; }

        public decimal TotalAfterDiscount { get; set; }
    }
}
