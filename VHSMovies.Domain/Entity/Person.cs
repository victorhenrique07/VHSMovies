﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Person
    {
        public Person(string externalId, string name, IReadOnlyCollection<Title> titles)
        {
            ExternalId = externalId;
            Name = name;
            Titles = titles;
        }

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public IReadOnlyCollection<Title> Titles { get; set; }
    }
}