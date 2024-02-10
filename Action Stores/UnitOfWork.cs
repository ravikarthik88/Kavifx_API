using Kavifx_API.Services.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Kavifx_API.Models;
using Kavifx_API.Action_Stores.Services;

namespace Kavifx_API.Services.Repository
{
    public class UnitOfWork
    {
        private readonly KavifxDbContext _context;        
        
        UserService _userService;

        public UnitOfWork(KavifxDbContext context)
        {
            _context = context;            
        }

        public UserService userService
        {
            get
            {
                if(_userService == null)
                {
                    _userService = new UserService(_context);
                }
                return _userService;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();            
        }
        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}
