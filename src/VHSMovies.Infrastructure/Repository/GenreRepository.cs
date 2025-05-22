using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure;

namespace VHSMovies.Infraestructure.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DbContextClass dbContextClass;

        public GenreRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IReadOnlyCollection<Genre>> GetAll()
        {
            return await dbContextClass.Genres.ToListAsync();
        }
    }
}
