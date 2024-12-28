using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Api.Integration.Detail;
using VHSMovies.Api.Integration.Filter;

namespace VHSMovies.Api.Integration.Client
{
    public interface ITitleClient
    {
        [Get("/title")]
        public Task<IReadOnlyCollection<TitleDetails>> GetTitles(TitleFilters filters);
    }
}
