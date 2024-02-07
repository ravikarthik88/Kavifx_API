using Kavifx_API.Models;
using KavifxApp.Server.Helpers;

namespace Kavifx_API.Services
{
    public class UserService
    {
        private readonly KavifxDbContext context;
        private readonly User user = new User();
        public UserService(KavifxDbContext dbContext)
        {
            context = dbContext;
        }

        public User ValidateUser(string email,string password)
        {
            var pwd = Bcrypt.Encryptpassword(password);
            var result = context.Users.Where(c => c.Email == email && c.Password == pwd && c.IsDeleted == false).FirstOrDefault();
            if(result != null)
            {
                user.Email = result.Email;
                user.Firstname = result.Firstname;
                user.Role = result.Role;
            }
            return user;
        }
    }
}
