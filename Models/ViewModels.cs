using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kavifx_API.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }        
        
        [Required,DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public byte[] ProfilePicture { get; set; }
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

    public class LoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
