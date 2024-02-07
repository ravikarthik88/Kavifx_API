using System.ComponentModel.DataAnnotations;

namespace Kavifx_API.Models
{
    public class LoginViewModel
    {
        [Required,EmailAddress(ErrorMessage ="Please Enter a valid Email Address")]
        public string Email { get; set; }
        [Required,DataType(DataType.Password),MinLength(15)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress(ErrorMessage = "Please Enter a valid Email Address")]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), MinLength(15)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password does not match"),DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; } = "User";

        [Required(ErrorMessage = "Please choose profile image")]
        public string ProfilePictureUrl { get; set; }
    }

    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }

    public class PermissionViewModel
    {
        public string PermissionName { get; set; }
    }

    public class AssignRoleToUserViewModel
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }

    public class UpdateRoleToUserViewModel
    {
        public string Email { get; set; }
        public List<string> RoleNames { get; set; }
    }

    public class RolePermissionViewModel
    {
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
    }

    public class UpdateRolePermissionModel
    {
        public string RoleName { get; set; }
        public List<string> PermissionNames { get; set; }
    }
}
