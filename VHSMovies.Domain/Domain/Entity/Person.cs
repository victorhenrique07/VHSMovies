using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Person
    {
        public Person() { }

        public Person(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Cast> Titles { get; set; } = new List<Cast>();

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
