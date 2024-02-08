using Kavifx_API.Action_Stores.Repository;
using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kavifx_API.Action_Stores.Services
{
    public class UserService : IUserService
    {
        private readonly KavifxDbContext ctx;
        private readonly UserRepository userRepo;
        public UserService(KavifxDbContext context,UserRepository userRepository)
        {
            ctx = context;
            userRepo = userRepository;
        }
        public Task<bool> CreateUserAsync(UserDTO userDTO)
        {
            var User = userRepo.Add(userDTO);
            return User;
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            var delete = userRepo.Delete(userId);
            return delete;
        }

        public bool EmailExists(string email)
        {
            var user = ctx.Users.Where(c => c.Email == email).FirstOrDefault();
            if(user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<List<UserDTO>> GetAllUsersAsync()
        {
            var UserList = userRepo.GetAll();
            return UserList;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await(from c in ctx.Users
                             where c.Email == email && c.IsDeleted == false
                             select new UserDTO
                             {
                                 UserId = c.UserId,
                                 Firstname = c.Firstname,
                                 LastName = c.LastName,
                                 Email = c.Email
                             }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await(from c in ctx.Users
                             where c.UserId == userId && c.IsDeleted == false
                             select new UserDTO
                             {
                                 UserId = c.UserId,
                                 Firstname = c.Firstname,
                                 LastName = c.LastName,
                                 Email = c.Email
                             }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> UpdateUserAsync(int userId, UserDTO userDTO)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();
            if(user != null)
            {
               var result = await userRepo.Update(userDTO);
               return true;
            }

            return false;
        }
    }
}
