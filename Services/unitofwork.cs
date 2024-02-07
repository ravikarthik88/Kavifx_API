using Kavifx_API.Models;
using Kavifx_API.Services.Repository;

namespace Kavifx_API.Services
{
    public class unitofwork
    {
        UserRepository userRepo;
        UserProfileRepository profileRepo;
        RoleRepository roleRepo;
        userRepository userRoleRepo;
        PermissionRepository permissionRepo;
        RolePermissionRepository rolePermissionRepos;
        KavifxDbContext context;

        public unitofwork(UserRepository userRepository, UserProfileRepository profileRepository,
             RoleRepository roleRepository,userRepository userRoleRepository,
             PermissionRepository permissionRepository, RolePermissionRepository rolePermissionRepository,
             KavifxDbContext dbContext
            )
        {
            userRepo = userRepository;
            profileRepo = profileRepository;
            roleRepo = roleRepository;
            userRoleRepo = userRoleRepository;
            permissionRepo = permissionRepository;
            rolePermissionRepos = rolePermissionRepository;
            context= dbContext;
        }

        public UserRepository repo
        {
            get{
                if(userRepo ==null)
                {
                    userRepo = new userRepository(context);
                };
                return userRepo;
            }
        }
        public RoleRepository profile
        {
            get
            {
                if (profileRepo == null)
                {
                    profileRepo = new profileRepository(context);
                }
                return profileRepo;
        }

    }
}
