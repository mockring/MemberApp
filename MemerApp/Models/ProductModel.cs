using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemberApp.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "必填")]
        [Range(0, int.MaxValue, ErrorMessage = "必須為整數。")]
        [Display(Name = "進貨價格")]
        public int PurchasePrice { get; set; }

        [Required(ErrorMessage = "必填")]
        [Range(0, int.MaxValue, ErrorMessage = "必須為整數。")]
        [Display(Name = "建議售價")]
        public int SuggestedPrice { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.Date;

    }
}
