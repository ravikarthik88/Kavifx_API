using Kavifx_API.Action_Stores.Repository;
using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using Kavifx_API.Services.Repository;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kavifx_API.Action_Stores.Services
{
    public class UserService : IUserService
    {
        private readonly KavifxDbContext ctx;
        private readonly UnitOfWork uw;
        public UserService(KavifxDbContext context,UnitOfWork unitOfWork)
        {
            ctx = context;            
        }

        public async Task<bool> CreateUserAsync(UserDTO userDTO)
        {
            var user = new User
            {
                Firstname = userDTO.Firstname,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = Bcrypt.Encryptpassword(userDTO.Password)
            };

            await ctx.Users.AddAsync(user);
            await uw.SaveChangesAsync();

            var pictureUrl = UploadProfilePicture(userDTO.ProfilePicture);

            var profile = new UserProfile()
            {
                UserId = user.UserId,
                PictureUrl = pictureUrl,
                UploadedAt = DateTime.Now
            };
            await ctx.Profiles.AddAsync(profile);
            await uw.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await(from c in ctx.Users
                        where c.UserId == userId && c.IsDeleted == false
                        select c).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsDeleted = true;
                ctx.Users.Update(user);
                await uw.SaveChangesAsync();
                return true;
            }

            return false;
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

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var Users = await(from c in ctx.Users
                              where c.IsDeleted == false
                              select new UserDTO
                              {
                                  UserId = c.UserId,
                                  Firstname = c.Firstname,
                                  LastName = c.LastName,
                                  Email = c.Email
                              }).ToListAsync();

            return Users;
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
                              where c.UserId == userDTO.UserId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Firstname = userDTO.Firstname;
                user.LastName = userDTO.LastName;
                user.Email = userDTO.Email;
                ctx.Users.Update(user);
                await uw.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private string UploadProfilePicture([FromBody] IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                string UploadDir = $"~/Uploads/ProfilePictures/";
                if (!Directory.Exists(UploadDir))
                {
                    Directory.CreateDirectory(UploadDir);
                }

                var fileName = Path.GetFileName(profilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), UploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    profilePicture.CopyToAsync(stream);
                }

                return filePath;
            }
            return string.Empty;
        }
    }
}
