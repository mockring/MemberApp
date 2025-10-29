using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace MemberApp.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "帳號")]
        public string Account { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Display(Name = "密碼雜湊")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Display(Name = "密碼種子")]
        public string PasswordSalt { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("密碼不能為空或全空格", nameof(plainPassword));

            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            PasswordSalt = Convert.ToBase64String(saltBytes);

            byte[] combined = Encoding.UTF8.GetBytes(plainPassword + PasswordSalt);
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(combined);
            PasswordHash = Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                return false;

            byte[] combined = Encoding.UTF8.GetBytes(plainPassword + PasswordSalt);
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(combined);
            string hashBase64 = Convert.ToBase64String(hash);

            return hashBase64 == PasswordHash;
        }
    }
}