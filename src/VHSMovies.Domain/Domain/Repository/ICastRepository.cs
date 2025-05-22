using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface ICastRepository : IRepository<Cast>
    {
        Task<IReadOnlyCollection<Cast>> GetAllCastByTitleAsync(int titleId);

        Task<IReadOnlyCollection<Cast>> GetCastsByPersonRole(PersonRole personRole);
    }
}
