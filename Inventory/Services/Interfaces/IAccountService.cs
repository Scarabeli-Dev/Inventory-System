using Inventory.Models.Account;
using Inventory.ViewModels.AccountVM;

namespace Inventory.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<Role>> GetAllRoles();
        Task<User> CreateUser(RegisterViewModel userVM);
    }
}
