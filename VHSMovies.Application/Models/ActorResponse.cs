using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Models
{
    public class ActorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TitleResponse> Titles { get; set; }
    }
}
