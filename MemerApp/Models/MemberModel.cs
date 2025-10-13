using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemerApp.Models
{
    /// 會員資料模型 (Code‑First)
    public class MemberModel
    {
        /// 自動遞增的主鍵
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        /// 姓名 (中文，必填，最多10字)
        [Required(ErrorMessage = "姓名為必填項目。")]
        [StringLength(10, ErrorMessage = "姓名最多只能輸入 10 個字。")]
        ///[RegularExpression(@"^[\u4e00-\u9fa5]+$", ErrorMessage = "姓名只能為中文。")]
        [Display(Name = "姓名")]
        public string MemberName { get; set; }

        /// 電話 (必填，7~15 個數字)
        [Required(ErrorMessage = "電話為必填項目。")]
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "電話號碼必須為 7-15 位數字。")]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        /// 生日（只存年月日）
        ///[Required(ErrorMessage = "生日為必填項目。")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]  // SQL Server date (不帶時間)
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        /// 創建日期（只存年月日，建立時自動填入）
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [Display(Name = "加入日期")]
        public DateTime CreatedDate { get; set; }

        /// 建構子：自動設定 CreatedDate 為目前時間（UTC）
        public MemberModel()
        {
            CreatedDate = DateTime.UtcNow.Date;
        }
    }
}
