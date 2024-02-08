using Kavifx_API.Services.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Kavifx_API.Models;

namespace Kavifx_API.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KavifxDbContext _context;        
        
        public UnitOfWork(KavifxDbContext context)
        {
            _context = context;            
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
