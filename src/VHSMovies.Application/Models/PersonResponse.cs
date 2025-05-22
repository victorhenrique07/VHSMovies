using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Models
{
    public class PersonResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<TitleResponse>? Titles { get; set; }

        public PersonResponse(int id, string name, ICollection<TitleResponse> titles)
        {
            Id = id;
            Name = name;
            Titles = titles;
        }
    }
}
