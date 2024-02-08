using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kavifx_API.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password),MinLength(15)]
        public string Password { get; set; }
        
        [Required,DataType(DataType.Password), MinLength(15)]
        [Compare("Password", ErrorMessage = "passwords doesnot match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage ="Please choose a file")]
        public IFormFile ProfilePicture { get; set; }
    }

  

    public class RoleDTO
    {   public int RoleId { get; set; }
        public string RoleName { get; set; }     
    }

    public class UserRoleDTO
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }        
    }

    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
    }

    public class RolePermissionDTO
    {
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }     
    }
}
