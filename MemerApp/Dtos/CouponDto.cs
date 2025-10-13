using MemerApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemerApp.Dtos
{
    /// 折扣券資料傳輸物件 (DTO)
    public class CouponDto
    {
        /// <summary>折扣券 ID（可選，創建時不填）</summary>
        public int CouponId { get; set; }

        /// <summary>折扣券名稱（必填）</summary>
        [Required(ErrorMessage = "折扣券名稱為必填欄位")]
        [StringLength(100, ErrorMessage = "折扣券名稱長度不可超過 100 個字")]
        [Display(Name = "折扣名稱")]
        public string CouponName { get; set; } = string.Empty;

        /// <summary>計算方式（必填，僅允許 Add/Subtract/Multiply/Divide）</summary>
        [Required(ErrorMessage = "計算方式為必填")]
        [EnumDataType(typeof(CalculationMethod),
            ErrorMessage = "計算方式必須是：Add、Subtract、Multiply 或 Divide")]
        [Display(Name = "計算方式")]
        public CalculationMethod CalculationMethod { get; set; }

        /// <summary>折扣數值（必填）</summary>
        [Required(ErrorMessage = "折扣數字為必填")]
        [Range(0, 100_000_000_000, ErrorMessage = "折扣數字必須為合法數值")]
        [Column(TypeName = "decimal(9,2)")]
        [Display(Name = "折扣數字")]
        public decimal DiscountValue { get; set; }

        /// <summary>備註（可選）</summary>
        [Display(Name = "備註")]
        public string? Remark { get; set; }
    }
}