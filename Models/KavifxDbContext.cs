using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security;
using System.Net.Mime;

namespace Kavifx_API.Models
{
    public class KavifxDbContext : DbContext
    {
        public KavifxDbContext(DbContextOptions<KavifxDbContext> opts):base(opts) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ProfilePicture> Profiles { get; set; }
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
        public bool IsDeleted { get; set; } = false;       

    }

    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string ProfilePictureUrl { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }

    public class ProfilePicture
    {
        [Key]
        public int ProfilePicId { get; set; }
        public int UserId { get; set; }
        public byte[] PictureData { get; set; }
        public string PictureMimeType { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsDeleted { get; set; } = false;
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
        public bool IsDeleted { get; set; } = false;
    }

    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
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
        public bool IsDeleted { get; set; } = false;
    }

    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        [ForeignKey("MenuId")]
        public int ParentId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class RoleMenu
    {
        [Key]
        public int RoleMenuId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
