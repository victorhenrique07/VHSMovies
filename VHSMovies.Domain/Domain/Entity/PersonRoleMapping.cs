using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class PersonRoleMapping
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public PersonRole Role { get; set; }
    }
}
