using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Infraestructure
{
    public interface IDataReader
    {
        IReadOnlyCollection<Title> ReadTitles();

        Title ReadTitle(string url);

        string GetSourceName();
    }
}
