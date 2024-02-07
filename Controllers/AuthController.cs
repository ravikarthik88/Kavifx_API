using Kavifx_API.Models;
using KavifxApp.Server.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly KavifxDbContext context;
        public AuthController(IConfiguration configuration, KavifxDbContext dbcontext)
        {
            config = configuration;
            context = dbcontext;
        }

        [HttpPost("Login"),AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel model)
        {           
            var result = context.Users.Where(c => c.Email == model.Email).FirstOrDefault();
            if (result == null || !VerifyPassword(result.Password, model.Password))
            {
                return BadRequest("Incorrect Password! Please check your password!");
            }
            string token = CreateToken(model.Email, model.Password);
            return Ok();
        }

        private bool VerifyPassword(string password1, string password2)
        {
            bool Verify = false;
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

        private string CreateToken(string emailId, string password)
        {
            User user = new User();
            user = uw.UserService.validateUser(emailId, password);
            var roles = context.UserRoles.Where(x => x.UserId == user.UserId && x.IsDeleted == false).Select(x => x.Role.RoleName).FirstOrDefault();
            if (user != null)
            {
                var handler = new JwtSecurityTokenHandler();
                byte[] Keybytes = System.Text.Encoding.UTF8.GetBytes(config.GetSection("JWTDATA:Key").Value);

                var Tdesc = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role,roles)
                    }),
                    Expires = DateTime.Now.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Keybytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var Token = handler.CreateToken(Tdesc);
                var TokenString = handler.WriteToken(Token);
                return TokenString;
            }
            return string.Empty;
        }
    }
}
