using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Infraestructure.Repository
{
    public class TitleRepository : ITitleRepository
    {
        private readonly DbContextClass dbContextClass;

        public TitleRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IEnumerable<Title>> GetAll(string reviewerName)
        {
            return await dbContextClass.Titles
                .Where(t => t.Ratings.Select(r => r.Reviewer == reviewerName) != null)
                .ToListAsync();
                //.Where(p => p.Ratings.Select( c => c.Reviewer == reviewerName) != null).ToListAsync();
        }

        public async Task<Title> GetByIdAsync(int id)
        {
            return await dbContextClass.Set<Title>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Title> GetByExternalIdAsync(string externalId)
        {
            return await dbContextClass.Set<Title>().FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public async Task UpdateByExternalIdAsync(Title title)
        {
            dbContextClass.Set<Title>().Update(title);
            await dbContextClass.SaveChangesAsync();
        }

        public async Task RegisterAsync(Title entity)
        {
            await dbContextClass.Set<Title>().AddAsync(entity);
            await dbContextClass.SaveChangesAsync();
        }
    }
}
