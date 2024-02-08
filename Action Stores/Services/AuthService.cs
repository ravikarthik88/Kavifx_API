using Kavifx_API.Action_Stores.Repository;
using Kavifx_API.Models;
using Kavifx_API.Services.Interface;

namespace Kavifx_API.Action_Stores.Services
{
   
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepo;
        public AuthService(UserRepository userRepo) 
        {
            _userRepo = userRepo;
        }
        public User Authenticate(string username, string password)
        {
            var user = ctx.Users.Where(c => c.Email == username).FirstOrDefault();
        }

        public bool IsInRole(User user, string role)
        {
            throw new NotImplementedException();
        }
    }
}
