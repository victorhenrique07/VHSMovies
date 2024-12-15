﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Person
    {
        public Person() { }

        public Person(string externalId, string name, string url)
        {
            ExternalId = externalId;
            Name = name;
            Url = url;
        }

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }
    }
}
