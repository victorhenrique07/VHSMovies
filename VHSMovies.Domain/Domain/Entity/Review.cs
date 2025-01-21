using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Review
    {
        public Review(string reviewer, decimal rating)
        {
            Reviewer = reviewer;
            Rating = rating;
        }

        public int Id { get; set; }

        public string Reviewer { get; set; }

        public decimal Rating { get; set; }

        public string TitleExternalId { get; set; }
    }
}
