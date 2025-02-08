using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Api.Integration.Main.Responses;

namespace VHSMovies.Api.Integration.Main.Clients
{
    public interface IGenresClient
    {
        [Get("/api/genres")]
        public Task<IReadOnlyCollection<GenreResponse>> GetAllGenres();
    }
}
