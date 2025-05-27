using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Models
{
    public class WatchProvider
    {
        public WatchProvider(string name, string logoUrl, string type)
        {
            Name = name;
            LogoUrl = $"https://image.tmdb.org/t/p/original{logoUrl}";
            Type = type;
        }

        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
