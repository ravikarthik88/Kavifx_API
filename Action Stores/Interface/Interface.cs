using Kavifx_API.Models;

namespace Kavifx_API.Services.Interface
{
    #region Services    

    public interface IUserRoleService
    {
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        List<Role> GetRolesForUser(int userId);
        List<User> GetUsersForRole(int roleId);
    }

    public interface IPermissionService
    {
        Task<List<PermissionViewModel>> GetAllPermissionsAsync();
        Task<bool> CreatePermissionAsync(PermissionViewModel model);
        Task<bool> DeletePermissionAsync(int permissionId);
    }

    public interface IRolePermissionService
    {
        Task<bool> AssignPermissionAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionAsync(int roleId, int permissionId);
        Task<List<PermissionViewModel>> GetRolePermissionsAsync(int roleId);
    }
   
    #endregion
}
