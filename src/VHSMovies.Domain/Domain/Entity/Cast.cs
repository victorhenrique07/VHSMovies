﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Cast
    {
        public int Id { get; set; }
        public Title Title { get; set; }
        public Person Person { get; set; }
        public PersonRole Role { get; set; }

        public Cast() { }

        public Cast(Title title, Person person, PersonRole role)
        {
            Title = title;
            Person = person;
            Role = role;
        }
    }
}
