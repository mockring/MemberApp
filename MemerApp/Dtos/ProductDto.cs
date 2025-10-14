using System.ComponentModel.DataAnnotations;

namespace MemerApp.Dtos
{
    /// <summary>
    /// 產品資料傳輸物件 (DTO)
    /// </summary>
    public class ProductDto
    {
        /// 產品 ID (主鍵，自動產生)
        public int ProductId { get; set; }

        /// 產品名稱 (必填)
        [Required(ErrorMessage = "產品名稱為必填項目。")]
        [StringLength(100, ErrorMessage = "產品名稱長度不能超過 100 個字元。")]
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }

        /// 進貨價格 (必填，整數)
        [Required(ErrorMessage = "進貨價格為必填項目。")]
        [Range(0, int.MaxValue, ErrorMessage = "進貨價格必須為整數。")]
        [Display(Name = "進貨價格")]
        public int PurchasePrice { get; set; }

        /// 建議售價 (必填，整數)
        [Required(ErrorMessage = "建議售價為必填項目。")]
        [Range(0, int.MaxValue, ErrorMessage = "建議售價必須為整數。")]
        [Display(Name = "建議售價")]
        public int SuggestedPrice { get; set; }

        /// 建立時間（只傳送年月日）
        [DataType(DataType.Date)]
        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; }
    }
}