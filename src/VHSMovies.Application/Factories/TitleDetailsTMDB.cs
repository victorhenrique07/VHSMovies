using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Factories
{
    public class TitleDetailsTMDB
    {
        public List<TitleDetailsResult> Movie_Results { get; set; } = new List<TitleDetailsResult>();
        public List<TitleDetailsResult> Tv_Results { get; set; } = new List<TitleDetailsResult>();
        public List<TitleDetailsResult> Tv_Season_Results { get; set; } = new List<TitleDetailsResult>();
    }
}
