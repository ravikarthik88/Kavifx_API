using Kavifx_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kavifx_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly KavifxDbContext ctx;       
        private readonly IWebHostEnvironment env;
        public UserProfileController(KavifxDbContext dbContext,IWebHostEnvironment environment)
        {
            ctx = dbContext;
            env = environment;
        }

        [HttpPost("Profile")]
        public async Task<IActionResult> CreateUserProfile(int UserId,UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userProfile1 = new UserProfile
                {
                    UserId = UserId,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    Organization_Name = model.Organization_Name,
                    Location = model.Location,
                    ProfilePictureUrl = model.ProfilePictureUrl
                };
                await ctx.UserProfiles.AddAsync(userProfile1);
                await ctx.SaveChangesAsync();
                return Ok("User Profile is Added Successfully");
            }

            return BadRequest("User Profile is not Added");
        }
    }
}
