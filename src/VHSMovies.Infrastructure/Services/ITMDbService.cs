using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Services.Responses;

namespace VHSMovies.Infrastructure.Services
{
    public interface ITMDbService
    {
        Task<TMDbWatchProvider> FindTitleWatchProvider(int titleId, TitleType titleType);

        Task<TitleDetailsTMDB> FindTitleDetails(string imdbId);
    }
}
