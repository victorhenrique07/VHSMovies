using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OpenQA.Selenium.BiDi.Modules.Log;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure;

namespace VHSMovies.Infraestructure.Repository
{
    public class TitleRepository : ITitleRepository
    {
        private readonly DbContextClass dbContextClass;

        public TitleRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<List<Title>> GetAll()
        {
            return await dbContextClass.Set<Title>()
                .Include(t => t.Genres)
                    .ThenInclude(tg => tg.Genre)
                .Include(t => t.Ratings)
                .ToListAsync();
        }

        public async Task<List<Title>> GetAllByReviewerName(string reviewerName)
        {
            return await dbContextClass.Set<Title>()
                .Include(t => t.Ratings)
                .Where(t => t.Ratings.Any(r => r.Reviewer == reviewerName))
                .ToListAsync();
        }

        public async Task<List<Title>> GetAllByGenreId(int genreId)
        {
            return await dbContextClass.Set<Title>()
                .Include(g => g.Genres)
                .Where(t => t.Genres.Any(r => r.Genre.Id == genreId))
                .ToListAsync();
        }

        public async Task<Title> GetByIdAsync(int id)
        {
            return await Query()
                .Include(t => t.Ratings)
                .Include(c => c.Casts)
                    .ThenInclude(p => p.Person)
                .Include(t => t.Genres)
                    .ThenInclude(tg => tg.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Title> GetByExternalIdAsync(string externalId)
        {
            return await dbContextClass.Set<Title>().FirstOrDefaultAsync(x => x.Ratings.Any(e => e.TitleExternalId == externalId));
        }

        public async Task RegisterAsync(Title entity)
        {
            try
            {
                await dbContextClass.Set<Title>().AddAsync(entity);

                Console.WriteLine($"Adding: {entity.ToString()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar dados: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterListAsync(List<Title> entity)
        {
            try
            {
                await dbContextClass.Set<Title>().AddRangeAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar dados: {ex.Message}");
                throw;
            }
        }

        public IQueryable<Title> Query()
        {
            return dbContextClass.Titles.AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
            await dbContextClass.SaveChangesAsync();
        }
    }
}
