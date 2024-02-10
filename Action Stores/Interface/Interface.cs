using Kavifx_API.Action_Stores.Services;
using Kavifx_API.Models;

namespace Kavifx_API.Services.Interface
{ 
    #region Services

    public interface IAuthService
    {
        User AuthenticateAsync(string username, string password);
        bool IsInRole(User user, string role);
    }
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> GetUserByEmailAsync(string email);        
        bool EmailExists(string email);
        Task<bool> CreateUserAsync(UserDTO userDTO);
        Task<bool> UpdateUserAsync(int userId, UserDTO userDTO);
        Task<bool> DeleteUserAsync(int userId);
    }    
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(RoleDTO roleDTO);
        Task<bool> UpdateRoleAsync(RoleDTO roleDTO);
        Task<bool> DeleteRoleAsync(int roleId);
    }

    public interface IUserRoleService
    {
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        List<Role> GetRolesForUser(int userId);
        List<User> GetUsersForRole(int roleId);
    }

    public interface IPermissionService
    {
        Task<List<PermissionDTO>> GetAllPermissionsAsync();
        Task<bool> CreatePermissionAsync(PermissionDTO permissionDTO);
        Task<bool> DeletePermissionAsync(int permissionId);
    }

    public interface IRolePermissionService
    {
        Task<bool> AssignPermissionAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionAsync(int roleId, int permissionId);
        Task<List<PermissionDTO>> GetRolePermissionsAsync(int roleId);
    }
   
    #endregion
}
