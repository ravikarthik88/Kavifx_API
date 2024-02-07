using Kavifx_API.Models;
using Kavifx_API.Services.Repository;

namespace Kavifx_API.Services
{
    public class unitofwork
    {
        UserRepository userRepo;
        UserService _user;
        KavifxDbContext context;

        public unitofwork(KavifxDbContext dbContext)
        {
            context = dbContext;
        }

        public UserRepository repo
        {
            get{
                if(userRepo ==null)
                {
                    userRepo = new UserRepository(context);
                };
                return userRepo;
            }
        }

    }
}
