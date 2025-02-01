using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Api.Integration.Main.Queries;
using VHSMovies.Api.Integration.Main.Responses;

namespace VHSMovies.Api.Integration.Main.Clients
{
    public interface ITitlesClient
    {
        [Get("/api/titles/most-relevant")]
        public Task<IReadOnlyCollection<TitleResponse>> GetMostRelevantTitles();

        [Get("/api/titles/most-relevant/{genre}")]
        public Task<IReadOnlyCollection<TitleResponse>> GetMostRelevantTitlesByGenre(string genre);

        [Get("/api/titles/recommend")]
        public Task<IReadOnlyCollection<TitleResponse>> GetRecommendationsTitles(GetRecommendedTitlesQuery query);

        [Get("/api/titles/{id}")]
        public Task<TitleResponse> GetTitleById(int id);
    }
}
