using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security;

namespace Kavifx_API.Models
{
    public class KavifxDbContext : DbContext
    {
        public KavifxDbContext(DbContextOptions<KavifxDbContext> opts):base(opts) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
    }


    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";
        public bool IsDeleted { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RolePermission
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
        public bool IsDeleted { get; set; }
    }
}
