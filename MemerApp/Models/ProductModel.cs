using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemerApp.Models
{
    public class ProductModel
    {
        /// 產品 ID (主鍵，Auto Increment)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// 產品名稱 (必填)
        [Required(ErrorMessage = "產品名稱為必填項目。")]
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }

        /// 進貨價格 (必填，整數，>0)
        [Required(ErrorMessage = "進貨價格為必填項目。")]
        [Range(0, int.MaxValue, ErrorMessage = "進貨價格必須為正整數。")]
        [Display(Name = "進貨價格")]
        public int PurchasePrice { get; set; }

        /// 建議售價 (必填，整數，>0)
        [Required(ErrorMessage = "建議售價為必填項目。")]
        [Range(0, int.MaxValue, ErrorMessage = "建議售價必須為正整數。")]
        [Display(Name = "建議售價")]
        public int SuggestedPrice { get; set; }

        /// 建立時間 (只存年月日)
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]   // SQL Server 的 date 型別，無時間成分
        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; }

        /// 建構子：自動設定 CreatedDate 為本日（UTC）
        public ProductModel()
        {
            CreatedDate = DateTime.UtcNow.Date;
        }
    }
}
