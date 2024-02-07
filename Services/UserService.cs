using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using Kavifx_API.Services.Repository;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Kavifx_API.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository user;
        
        public UserService(UserRepository repository)
        {
            user = repository;
        }

        public async Task<bool> AddUser([FromBody] RegisterViewModel model)
        {
            bool AddedUser = await user.AddUser(model);
            if (AddedUser)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetbyId(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User ValidateUser(string email,string password)
        {
            var validuser = user.ValidateUser(email, password);
            return validuser;
        }
    }
}
