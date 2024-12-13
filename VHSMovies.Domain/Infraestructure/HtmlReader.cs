using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Infraestructure
{
    public class HtmlReader : IHtmlReader
    {
        private const string UserAgent = "Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Microsoft; Lumia 950) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Mobile Safari/537.36 Edge/13.1058";

        public HtmlReader() { }

        public HtmlDocument Read(string url)
        {
            HtmlWeb web = new HtmlWeb { UserAgent = UserAgent };

            return web.Load(url);
        }
    }
}
