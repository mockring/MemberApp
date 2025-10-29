using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemberApp.Models
{
    /// 會員資料模型 (Code‑First)
    public class MemberModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "必填")]
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string MemberName { get; set; } = string.Empty;

        [Required(ErrorMessage = "必填")]
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "必須為 7-15 位數字。")]
        [Display(Name = "電話")]
        public string Phone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [Display(Name = "加入日期")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.Date;

    }
}
