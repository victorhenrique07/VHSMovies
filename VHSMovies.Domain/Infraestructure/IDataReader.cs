using HtmlAgilityPack;
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
        string GetSourceName();

        string GetFullUrl(string externalId);

        Review ReadReview(string url);

        List<TitleGenre> ReadGenres(string url, int titleId);

        List<TitleGenre> ReadTitleGenres(HtmlDocument document, int titleId);
    }
}
