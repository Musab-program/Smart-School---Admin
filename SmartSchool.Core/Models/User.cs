using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartSchool.Core.Models
{
    // User extends IdentityUser<int> to leverage ASP.NET Core Identity features:
    // - Built-in password hashing (PasswordHash)
    // - Security stamp, concurrency stamp
    // - Email confirmation, lockout, two-factor auth
    // - int key to match existing DB schema
    public class User : IdentityUser<int>
    {
        // Display name (different from UserName which is used for login)
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        [Required, MaxLength(150)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string gender { get; set; }

        // Refresh Token fields for JWT refresh flow
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // Custom business role (direct FK for easy querying - one role per user)
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Guardian Guardian { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}
