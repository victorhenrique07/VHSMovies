using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Infrastructure.Services.Responses
{
    public class ProviderRegion
    {
        public List<ProviderDetails> flatrate { get; set; } = new List<ProviderDetails>();
        public List<ProviderDetails> rent { get; set; } = new List<ProviderDetails>();
    }
}
