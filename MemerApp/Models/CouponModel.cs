using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemberApp.Models
{
    /// 折扣券資料模型
    public class CouponModel
    {
        /// 折扣券 ID（自動產生）
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }

        /// 折扣券名稱（必填）
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(100)]
        [Display(Name = "折扣名稱")]
        public string CouponName { get; set; } = string.Empty;

        [Required(ErrorMessage = "必填")]
        [EnumDataType(typeof(CalculationMethod))]
        [Display(Name = "計算方式")]
        public CalculationMethod CalculationMethod { get; set; }

        [Required(ErrorMessage = "必填")]
        [Range(0,100000000000, ErrorMessage = "超過上限金額")]
        [Column(TypeName = "decimal(9,2)")]
        [Display(Name = "折扣數字")]
        public decimal CouponValue { get; set; }

        [Display(Name = "備註")]
        public string? Remark { get; set; }
    }

    public enum CalculationMethod
    {
        減, //0
        乘 //1
    }
}