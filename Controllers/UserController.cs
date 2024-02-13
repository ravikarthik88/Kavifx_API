using Kavifx_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kavifx_API.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]    
    public class UserController : ControllerBase
    {
        private readonly KavifxDbContext ctx;

        public UserController(KavifxDbContext dbContext)
        {
            ctx = dbContext;
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsDeleted = true;
                ctx.Users.Update(user);
                await ctx.SaveChangesAsync();
                return Ok("User Is Deleted Successfully");
            }
            return BadRequest("User Is not Deleted");
        }

        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            var Users = await (from c in ctx.Users
                               where c.IsDeleted == false
                               select c).ToListAsync();

            return Users;
        }

        [HttpGet("{email}")]
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await (from c in ctx.Users
                              where c.Email == email && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();

            return user;
        }

        [HttpGet("{userId}")]
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == userId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();

            return user;
        }

        [HttpPut("{userId}")]
        public async Task<bool> UpdateUserAsync(int UserId, RegisterViewModel model)
        {
            var user = await (from c in ctx.Users
                              where c.UserId == UserId && c.IsDeleted == false
                              select c).FirstOrDefaultAsync();
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                ctx.Users.Update(user);
                return true;
            }
            return false;
        }
    }
}
