﻿using Kavifx_API.Models;
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

        public async Task<bool> CreateUserAsync(UserDTO userDTO)
        {            
            var user = new User
            {
                Firstname = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = Bcrypt.Encryptpassword(userDTO.Password)                
            };

            await ctx.Users.AddAsync(user);

            string UploadUrl = await UploadImage(userDTO);
            var userprofile = new UserProfile
            {
                UserId = user.UserId,
                ProfilePictureUrl = UploadUrl
            };

            await ctx.UserProfiles.AddAsync(userprofile);

            byte[] imageBytes = File.ReadAllBytes(UploadUrl);
            var profiledata = new ProfilePicture
            {
                PictureData = imageBytes,
                PictureMimeType = userDTO.ProfilePicture.ContentType
            };

            await ctx.Profiles.AddAsync(profiledata);
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

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var Users = await (from c in ctx.Users
                               where c.IsDeleted == false
                               select new UserDTO
                               {
                                   UserId = c.UserId,
                                   FirstName = c.Firstname,
                                   LastName = c.LastName,
                                   Email = c.Email
                               }).ToListAsync();

            return Users;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await (from c in ctx.Users
                              where c.Email == email && c.IsDeleted == false
                              select new UserDTO
                              {
                                  UserId = c.UserId,
                                  FirstName = c.Firstname,
                                  LastName = c.LastName,
                                  Email = c.Email
                              }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select new UserDTO
                              {
                                  UserId = c.UserId,
                                  FirstName = c.Firstname,
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
                user.Firstname = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.Email = userDTO.Email;
                ctx.Users.Update(user);
                return true;
            }
            return false;
        }

        private async Task<string> UploadImage(UserDTO userDTO)
        {
            var Users = ctx.Users.SingleOrDefault(x => x.Email == userDTO.Email );

            if (userDTO.ProfilePicture != null && userDTO.ProfilePicture.Length > 0)
            {
                var UploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(UploadDir))
                {
                    Directory.CreateDirectory(UploadDir);
                }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(userDTO.ProfilePicture.FileName);
                var filepath = Path.Combine(UploadDir, filename);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await userDTO.ProfilePicture.CopyToAsync(stream);
                }
                return filepath;
            }
            return string.Empty;
        }
    }
}
