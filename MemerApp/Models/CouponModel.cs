using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemerApp.Models
{
    /// 折扣券資料模型
    public class CouponModel
    {
        /// 折扣券 ID（自動產生）
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }

        /// 折扣券名稱（必填）
        [Required(ErrorMessage = "折扣券名稱為必填欄位")]
        [StringLength(100, ErrorMessage = "折扣券名稱長度不可超過 100 個字")]
        [Display(Name = "折扣名稱")]
        public string CouponName { get; set; }

        /// 計算方式（必填，僅允許減、乘）
        [Required(ErrorMessage = "計算方式為必填")]
        [EnumDataType(typeof(CalculationMethod), 
            ErrorMessage = "計算方式必須是：減 或 乘")]
        [Display(Name = "計算方式")]
        public CalculationMethod CalculationMethod { get; set; }

        /// 折扣數字（必填，僅允許數字）
        [Required(ErrorMessage = "折扣數字為必填")]
        [Range(0,100000000000, ErrorMessage = "折扣數字必須為合法數值")]
        [Column(TypeName = "decimal(9,2)")]
        [Display(Name = "折扣數字")]
        public decimal CouponValue { get; set; }

        /// 備註（可選）
        [Display(Name = "備註")]
        public string? Remark { get; set; }
    }

    /// 計算方式枚舉，限制只能使用減、乘
    public enum CalculationMethod
    {
        減, //0
        乘 //1
    }
}