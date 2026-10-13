using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace SmartSchool.Core.Models
{
    // Role extends IdentityRole<int> to be recognized by ASP.NET Core Identity.
    // IdentityRole already provides: Id (int), Name, NormalizedName, ConcurrencyStamp.
    // We keep our custom navigation property for querying users with their role.
    public class Role : IdentityRole<int>
    {
        [JsonIgnore]
        public ICollection<User> Users { get; set; } // Navigation: Role(1) → User(n)
    }
}
