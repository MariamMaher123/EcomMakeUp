using EcomMakeUp.Dtos;
using EcomMakeUp.Models;

namespace EcomMakeUp.Servies
{
    public interface IAuthServies
    {
        Task<AuthModel> createUser(ApplicationUser user , string password);
        Task<IEnumerable<ApplicationUser>> listUsersByName(string name);
        ApplicationUser deleteUser(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> listUsers();
        Task<AuthModel> CheckUserLogin (LoginDTO UserData);
        Task<ApplicationUser> SelectUserById(string id);
        Task<ApplicationUser> SelectUserByEmail(string email);
        Task<string> AddRole(AddRoleDto dto);
        Task<AuthModel> changePassword (ChangePasswordDTO dto);
        Task<AuthModel> updateUser (ApplicationUser user);

    }
}
