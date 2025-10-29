using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemberApp.Dtos
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string MemberName { get; set; }
        [Phone]
        [Display(Name = "電話")]
        public string Phone { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]  // SQL Server date (不帶時間)
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [Display(Name = "加入日期")]
        public DateTime CreatedDate { get; set; }
    }
}
