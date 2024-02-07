using System.ComponentModel.DataAnnotations;

namespace Kavifx_API.Models
{
    public class LoginModel
    {
        [Required,EmailAddress(ErrorMessage ="Please Enter a valid Email Address")]
        public string Email { get; set; }
        [Required,DataType(DataType.Password,ErrorMessage ="Please Enter 1 UpperCase,1 lowercase, 1 symbols Maximum of 15 chars"),
            MinLength(8)]
        public string Password { get; set; }
    }
}
