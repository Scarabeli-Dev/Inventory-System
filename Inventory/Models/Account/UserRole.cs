using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Inventory.Models.Account
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}