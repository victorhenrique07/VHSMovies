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
        public Task<List<TitleResponse>> GetMostRelevantTitles(GetMostRelevantTitlesQuery query);

        [Get("/api/titles/recommend")]
        public Task<List<TitleResponse>> GetRecommendationsTitles(GetRecommendedTitlesQuery query);

        [Get("/api/titles/{id}")]
        public Task<TitleResponse> GetTitleById(int id);

        [Get("/api/search")]
        public Task<List<TitleResponse>> GetTitlesBySearch(string query);
    }
}
