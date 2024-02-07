using Kavifx_API.Models;

namespace Kavifx_API.Services.Interface
{
    #region Interface of Repository
    public interface IUserRepository
    {
        Task<bool> AddUser(RegisterViewModel model);
        User ValidateUser(string email, string password);
    }    
    #endregion

    public interface IUserService
    {
        List<User> GetAll();
        User GetbyId(int id); 
        User GetUserByEmail(string email);
        Task<bool> AddUser(RegisterViewModel model);

        User ValidateUser(string email, string password);
    }
}
