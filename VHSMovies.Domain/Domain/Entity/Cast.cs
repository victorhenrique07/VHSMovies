using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Cast
    {
        public int Id { get; set; }

        public ICollection<Person> Directors { get; set; } = new List<Person>();

        public ICollection<Person> Writers { get; set; } = new List<Person>();

        public ICollection<Person> Actors { get; set; } = new List<Person>();

        public Cast() { }

        public Cast(ICollection<Person> directors, ICollection<Person> writers, ICollection<Person> actors)
        {
            Directors = directors;
            Writers = writers;
            Actors = actors;
        }
    }
}
