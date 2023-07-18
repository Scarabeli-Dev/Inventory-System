using Inventory.Models.Account;
using Inventory.ViewModels.AccountVM;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<Role>> GetAllRoles();
        Task<User> CreateUser(RegisterViewModel userVM);
        Task<PagingList<User>> GetUsersByPaggingList(string filter, int pageindex = 1, string sort = "Name");
        Task<User> GetUserByIdAsync(int userId);
        bool UpdateUser(User user, ChangePasswordViewModel userVM);
        Task<bool> AdminUpdateUser(User user, EditUserViewModel userVM);
    }
}
