using System.ComponentModel.DataAnnotations;

namespace MemberApp.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}