using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Review
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }

        public int ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }

        public string TitleExternalId { get; set; }

        public string TitleExternalUrl { get; set; }

        public decimal Rating { get; set; }
    }
}
