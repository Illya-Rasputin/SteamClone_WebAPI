using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DeveloperRepository : GenericRepository<DeveloperEntity>
    {
        private readonly AppDbContext _context;
        public DeveloperRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DeveloperEntity> Developers => GetAll();
        public async Task<bool> IsExistsAsync(string name)
        {
            return await Developers
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());

        }

        public async Task<bool> IsExistsAsync(string name, params int[] exceptionIds)
        {
            return await Developers
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower()
                && !exceptionIds.Contains(d.Id));

        }

        

        public async Task<DeveloperEntity?> GetByNameAsync(string name)
        {
            return await _context.Developers
                .FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }
    }
}
