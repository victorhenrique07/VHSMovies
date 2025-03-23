using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface ITitleGenreRepository
    {
        Task RegisterGenresList(List<TitleGenre> genres);

        Task<List<TitleGenre>> GetTitleGenresById(int id);

        Task<List<TitleGenre>> GetTitlesByGenreId(int[] genresIds);
    }
}
