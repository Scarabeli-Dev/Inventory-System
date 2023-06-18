using Inventory.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Helpers
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedUserRoleInitial(UserManager<User> userManager, 
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            if(!_roleManager.RoleExistsAsync("Member").Result)
            {
                Role role = new Role();
                role.Name = "Member";
                role.NormalizedName = "MEMBER";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                Role role = new Role();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        public void SeedUsers()
        {
            if(_userManager.FindByEmailAsync("usuario@localhost").Result == null)
            {
                User user = new User();
                user.Name = "Teste Member";
                user.UserName = "usuario@localhost";
                user.Email = "usuario@localhost";
                user.NormalizedUserName = "USUARIO@LOCALHOST";
                user.NormalizedEmail = "USUARIO@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user,"Numsey#2022").Result;

                if(result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user,"Member").Wait();
                }
            }

            if (_userManager.FindByEmailAsync("elana.scarabeli@rcproconsultoria.com.br").Result == null)
            {
                User user = new User();
                user.Name = "Elanã";
                user.UserName = "esesena";
                user.Email = "elana.scarabeli@rcproconsultoria.com.br";
                user.NormalizedUserName = "ESESENA";
                user.NormalizedEmail = "ELANA.SCARABELI@RCPROCONSULTORIA.COM.BR";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "123456").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
