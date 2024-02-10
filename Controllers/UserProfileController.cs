using Kavifx_API.Models;
using Kavifx_API.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kavifx_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly KavifxDbContext ctx;       
        private readonly IWebHostEnvironment env;
        public UserProfileController(KavifxDbContext dbContext,UnitOfWork unitOfWork,IWebHostEnvironment environment)
        {
            ctx = dbContext;
            env = environment;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(int UserId,IFormFile profilePicture)
        {

            if(profilePicture!=null && profilePicture.Length > 0)
            {
                var UploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(UploadDir))
                {
                    Directory.CreateDirectory(UploadDir);
                }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                var filepath = Path.Combine(UploadDir, filename);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }
                var userProfile1 = new UserProfile
                {                  
                    ProfilePictureUrl = filepath
                };
                await ctx.UserProfiles.AddAsync(userProfile1);
                await ctx.SaveChangesAsync();
            }

            return Ok("Image Uploaded Successfully");
        }
    }
}
