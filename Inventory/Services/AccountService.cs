using Inventory.Data;
using Inventory.Models.Account;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

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
                _userManager.AddToRoleAsync(user, userVM.Role).Wait();
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task<PagingList<User>> GetUsersByPaggingList(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Users.Include(ur => ur.UserRole)
                                       .ThenInclude(r => r.Role)
                                       .AsNoTracking()
                                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Email.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
