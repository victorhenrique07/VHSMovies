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

        public Person(string externalId, string name, string url, List<PersonRoleMapping> roles)
        {
            ExternalId = externalId;
            Name = name;
            Url = url;
            Roles = roles;
        }

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public List<PersonRoleMapping> Roles { get; set; }

        public ICollection<Cast> Titles { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   ExternalId == person.ExternalId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExternalId);
        }
    }
}
