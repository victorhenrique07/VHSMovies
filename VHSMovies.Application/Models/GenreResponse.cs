using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Models
{
    public class GenreResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GenreResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
