using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text;

namespace Kavifx_API.Action_Stores.Services
{
    public class UserService : IUserService
    {
        private readonly KavifxDbContext ctx;        
        public UserService(KavifxDbContext context)
        {
            ctx = context;
        }

        public async Task<bool> CreateUserAsync(UserViewModel model)
        {            
            var user = new User
            {
                Firstname = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = Bcrypt.Encryptpassword(model.Password)                
            };
            await ctx.Users.AddAsync(user);
            var profile = new UserProfile
            {
                UserId = user.UserId,
                PictureURL = model.ProfilePictureUrl
            };
            await ctx.UserProfiles.AddAsync(profile);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsDeleted = true;
                ctx.Users.Update(user);
                return true;
            }

            return false;
        }

        public bool EmailExists(string email)
        {
            var user = ctx.Users.Where(c => c.Email == email).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var Users = await (from c in ctx.Users
                               where c.IsDeleted == false
                               select new UserViewModel
                               {
                                   UserId = c.UserId,
                                   FirstName = c.Firstname,
                                   LastName = c.LastName,
                                   Email = c.Email
                               }).ToListAsync();

            return Users;
        }

        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            var user = await (from c in ctx.Users
                              where c.Email == email && c.IsDeleted == false
                              select new UserViewModel
                              {
                                  UserId = c.UserId,
                                  FirstName = c.Firstname,
                                  LastName = c.LastName,
                                  Email = c.Email
                              }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select new UserViewModel
                              {
                                  UserId = c.UserId,
                                  FirstName = c.Firstname,
                                  LastName = c.LastName,
                                  Email = c.Email
                              }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> UpdateUserAsync(int userId, UserViewModel model)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == model.UserId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Firstname = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                ctx.Users.Update(user);
                return true;
            }
            return false;
        }       
    }
}
