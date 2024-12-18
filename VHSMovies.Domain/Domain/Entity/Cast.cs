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

        public int TitleId { get; set; }
        public Title Title { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public Cast() { }

        public Cast(Person person, Title title)
        {
            this.Person = person;
            this.Title = title;
        }

        public override bool Equals(object obj)
        {
            return obj is Cast cast &&
                   EqualityComparer<Person>.Default.Equals(Person, cast.Person) &&
                   EqualityComparer<Title>.Default.Equals(Title, cast.Title);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Person, Title);
        }
    }
}
