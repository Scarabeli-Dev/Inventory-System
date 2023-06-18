using Microsoft.AspNetCore.Identity;

namespace Inventory.Models.Account
{
    public class Role : IdentityRole<int>
    {
        public IEnumerable<UserRole> UserRole { get; set; }
    }
}