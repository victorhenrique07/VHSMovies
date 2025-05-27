using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Infrastructure.Services.Responses;

namespace VHSMovies.Infrastructure.Services
{
    public class TMDbWatchProvider
    {
        public Dictionary<string, WatchProviderResult> results { get; set; }
    }
}
