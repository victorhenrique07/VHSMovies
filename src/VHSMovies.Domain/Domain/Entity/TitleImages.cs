using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class TitleImages
    {
        public TitleImages(string principalImageUrl, string posterImageUrl)
        {
            this.PrincipalImageUrl = $"https://image.tmdb.org/t/p/original" + principalImageUrl;
            this.PosterImageUrl = $"https://image.tmdb.org/t/p/original" + posterImageUrl;
        }

        public TitleImages()
        {
        }

        public string PrincipalImageUrl { get; set; }

        public string PosterImageUrl { get; set; }
    }
}
