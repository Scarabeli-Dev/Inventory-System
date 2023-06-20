using Inventory.Data;
using Inventory.Models.Account;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Xml.Linq;

namespace Inventory.Services
{
    public class AccountService : IAccountService
    {
        private readonly InventoryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(InventoryContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<User> CreateUser(RegisterViewModel userVM)
        {
            User user = new User();
            user.UserName = userVM.UserName;
            user.Name = userVM.Name;
            user.Email = userVM.Email;
            
            IdentityResult result = _userManager.CreateAsync(user, userVM.Password).Result;

            if (result.Succeeded)
            {
                var role = await GetRoleByNameAsync(userVM.Role);
                var userReturn = await GetUserByUserNameAsync(user.UserName);

                UserRole userRole = new UserRole();
                userRole.UserId = userReturn.Id;
                userRole.RoleId = role.Id;

                _context.UserRoles.Add(userRole);
                _context.SaveChanges();
            }

            return user;
        }

        public async Task<PagingList<User>> GetAllUsersByPaggingList(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Users.Include(ur => ur.UserRole)
                                       .ThenInclude(r => r.Role)
                                       .AsNoTracking()
                                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) );
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


        //Private
        private async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(r => r.UserName == userName);
        }

        private async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
