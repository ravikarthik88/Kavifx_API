using Kavifx_API.Models;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kavifx_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly KavifxDbContext ctx;        
        public AuthController(IConfiguration configuration,KavifxDbContext context)
        {
            config = configuration;
            ctx = context;            
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await AuthenticateUser(login.email, login.password);
            if (user == null)
            {
                return Unauthorized();
            }
            var roles = GetUserRoles(user.UserId);
            var token = GenerateJwtToken(user.Email, roles);
            return Ok(new { token });
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (await UserExists(model.Email))
                {
                    return BadRequest("User Already Exists");
                }
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = Bcrypt.Encryptpassword(model.Password)
                };
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();
                return Ok("User Is Added Successfully");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }      

        private async Task<bool> UserExists(string email)
        {
            bool IsExists = false;
            var userExists = await ctx.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userExists != null) 
            {
                IsExists = true;
            }
            return IsExists;
        }

        private async Task<User> AuthenticateUser(string email,string password)
        {
            var user = await ctx.Users.SingleOrDefaultAsync(c => c.Email == email);
            if(user!=null && VerifyPassword(user.Password, password))
            {
                return user;
            }

            return null;
        }

        private List<string> GetUserRoles(int UserId)
        {
            var UserRoles = ctx.UserRoles.Where(ur => ur.UserId == UserId).Select(c => c.Role.RoleName).ToList();
            return UserRoles;
        }

        private string GenerateJwtToken(string email,List<string> roles)
        {
            var refreshToken = Guid.NewGuid().ToString();
            var handler = new JwtSecurityTokenHandler();
            byte[] Keybytes = System.Text.Encoding.UTF8.GetBytes(config.GetSection("JWTDATA:Key").Value);

            var Tdesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, string.Join(",", roles)),
                             new Claim("RefreshToken",refreshToken)
                }),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Keybytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var Token = handler.CreateToken(Tdesc);
            var TokenString = handler.WriteToken(Token);
            return TokenString;
        }

        private bool VerifyPassword(string password1,string password2)
        {
            bool Verify = false;
            if (!string.IsNullOrEmpty(password1))
            {
                
                string DecryptPassword = Bcrypt.Decryptpassword(password1);
                if (DecryptPassword == password2)
                {
                    Verify = true;
                    return Verify;
                }
                else
                {
                    Verify = false;
                    return Verify;
                }
            }
            return Verify;
        }              
    }
}
