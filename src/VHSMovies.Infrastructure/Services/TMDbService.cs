using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using MySqlX.XDevAPI;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Services;
using VHSMovies.Infraestructure.Services.Responses;

namespace VHSMovies.Infrastructure.Services
{
    public class TMDbService : ITMDbService
    {
        private readonly HttpClient httpClient;

        public TMDbService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TMDbWatchProvider> FindTitleWatchProvider(int titleId, TitleType type)
        {
            string titleType = type == TitleType.Movie ? "movie" : "tv";

            var response = await httpClient.GetAsync($"{titleType}/{titleId}/watch/providers");

            var json = await response.Content.ReadAsStringAsync();

            var t = JsonSerializer.Deserialize<TMDbWatchProvider>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return t;
        }

        public async Task<TitleDetailsTMDB> FindTitleDetails(string imdbId)
        {
            var response = await httpClient.GetAsync($"find/{imdbId}?external_source=imdb_id");

            var json = await response.Content.ReadAsStringAsync();

            var titleDetails = JsonSerializer.Deserialize<TitleDetailsTMDB>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return titleDetails;
        }
    }
}
