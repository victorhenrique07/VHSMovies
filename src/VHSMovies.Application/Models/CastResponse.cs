using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Models
{
    public class CastResponse
    {
        public PersonResponse Person { get; set; }
        public PersonRole Role { get; set; }
    }
}
