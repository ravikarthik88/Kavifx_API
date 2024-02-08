using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using Kavifx_API.Services.Repository;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kavifx_API.Action_Stores.Repository
{
    
    public class UserRepository : IRepository<UserDTO>
    {
        private readonly KavifxDbContext ctx;
        private readonly UnitOfWork uw;
        public UserRepository(KavifxDbContext context,UnitOfWork unitOfWork)
        {
            ctx = context;
        }

        public async Task<bool> Add([FromBody]UserDTO entity)
        {
            var user = new User
            {
                Firstname = entity.Firstname,
                LastName = entity.LastName,
                Email = entity.Email,
                Password = Bcrypt.Encryptpassword(entity.Password)
            };

            await ctx.Users.AddAsync(user);
            await uw.SaveChangesAsync();

            var pictureUrl = UploadProfilePicture(entity.ProfilePicture);

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

        public async Task<List<UserDTO>> GetAll()
        {
            var Users = await (from c in ctx.Users
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

        public UserDTO GetById(int id)
        {
            var user = (from c in ctx.Users
                        where c.IsDeleted == false && c.UserId == id
                        select new UserDTO
                        {
                            UserId = c.UserId,
                            Firstname = c.Firstname,
                            LastName = c.LastName,
                            Email = c.Email
                        }).FirstOrDefault();
            return user;
        }

        public async Task<bool> Update(UserDTO entity)
        {
            var user = await(from c in ctx.Users
                        where c.UserId == entity.UserId && c.IsDeleted == false
                        select c).FirstOrDefaultAsync();
            if (user !=null)
            {
                user.Firstname = entity.Firstname;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                ctx.Users.Update(user);
                await uw.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var user = (from c in ctx.Users
                        where c.UserId == id && c.IsDeleted == false
                        select c).FirstOrDefault();

            if(user != null)
            {
                user.IsDeleted = true;
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
