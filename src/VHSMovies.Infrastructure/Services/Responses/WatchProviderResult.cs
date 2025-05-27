using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Infrastructure.Services.Responses
{
    public class WatchProviderResult
    {
        public List<ProviderDetails> flatrate { get; set; }
        public List<ProviderDetails> rent { get; set; }
    }
}
