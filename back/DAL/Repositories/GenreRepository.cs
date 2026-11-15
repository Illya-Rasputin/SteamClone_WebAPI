using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace DAL.Repositories
{
    public class GenreRepository: GenericRepository<GenreEntity>
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<GenreEntity> Genres => GetAll();

        public async Task<bool> IsExistsAsync(string name)
        {
            return await Genres
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());

        }
        public async Task<bool> IsExistsAsync(string name, params int[] exceptionIds)
        {
            return await Genres
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower()
                && !exceptionIds.Contains(d.Id));

        }

        
        public async Task<GenreEntity?> GetByNameAsync(string name)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExitsAsync(string name)
        {
            return await GetByNameAsync(name) != null;
        }
    }
}
