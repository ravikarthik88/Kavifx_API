using Kavifx_API.Models;
using Kavifx_API.Services.Interface;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Kavifx_API.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KavifxDbContext context;
        private readonly User user = new User();
        public UserRepository(KavifxDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<bool> AddUser([FromBody] RegisterViewModel model)
        {
            try
            {              
                var user = new User()
                {
                    Firstname = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = Bcrypt.Encryptpassword(model.Password),
                    Role = model.Role,
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(model.ProfilePictureUrl))
                {
                    var pictureUrl = await UploadFileAsync(model.ProfilePictureUrl);

                    var profilePicture = new UserProfile
                    {
                        UserId = user.UserId,
                        PictureUrl = pictureUrl,
                        UploadedAt = DateTime.Now
                    };

                    context.Profiles.Add(profilePicture);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User ValidateUser(string email, string password)
        {
            var pwd = Bcrypt.Encryptpassword(password);
            var result = context.Users.Where(c => c.Email == email && c.Password == pwd && c.IsDeleted == false).FirstOrDefault();
            if (result != null)
            {
                user.Email = result.Email;
                user.Firstname = result.Firstname;
                user.Role = result.Role;
            }
            return user;
        }

        private async Task<string> UploadFileAsync(string pictureUrl)
        {
            using(var httpClient= new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(pictureUrl);
                var filename = Path.GetFileName(pictureUrl);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", filename);
                await File.WriteAllBytesAsync(filepath,imageBytes);
                return filepath;
            }
        }         
    }
}
