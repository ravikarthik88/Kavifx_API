using Kavifx_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Kavifx_API.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    [EnableCors("CrossPolicy")]
    public class RoleController : ControllerBase
    {
        private readonly KavifxDbContext ctx;

        public RoleController(KavifxDbContext dbContext)
        {
            ctx = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Roles = await ctx.Roles.Where(x => x.IsDeleted == false).ToListAsync();
            return Ok(Roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var role = await ctx.Roles.FindAsync(id);
            return Ok(role);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var role = await ctx.Roles.FindAsync(name);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                if (await RoleExists(role.RoleName))
                {
                    return BadRequest("Role Already Exists");
                }
                var role1 = new Role
                {
                    RoleName = role.RoleName
                };
                await ctx.Roles.AddAsync(role1);
                await ctx.SaveChangesAsync();
                return Ok("Role is Added Successfully");
            }
            return BadRequest("Role is Not Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoleViewModel role)
        {
            var selectedrole = await (from c in ctx.Roles
                                      where c.RoleId == role.RoleId && c.IsDeleted == false
                                      select c).FirstOrDefaultAsync();
            if(selectedrole != null)
            {
                selectedrole.RoleName = role.RoleName;
                ctx.Roles.Update(selectedrole);
                await ctx.SaveChangesAsync();
                return Ok("Role is Updated Successfully");
            }
            return BadRequest("Role is Not Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            var selectedrole = await (from c in ctx.Roles
                                      where c.RoleId == roleId && c.IsDeleted == false
                                      select c).FirstOrDefaultAsync();
            if (selectedrole != null)
            {
                selectedrole.IsDeleted = true;
                ctx.Roles.Update(selectedrole);
                await ctx.SaveChangesAsync();
                return Ok("Role is Deleted Successfully");
            }
            return BadRequest("Role is Not Deleted");
        }

        private async Task<bool> RoleExists(string RoleName)
        {
            bool IsExists = false;
            var role = await ctx.Roles.FindAsync(RoleName);
            if (role != null)
            {
                IsExists = true;
            }
            return IsExists;
        }
    }
}
