using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Cast
    {
        public int ActorId { get; set; }

        public virtual Actor Actor { get; set; }

        public int TitleId { get; set; }

        public virtual Title Title { get; set; }
    }
}
