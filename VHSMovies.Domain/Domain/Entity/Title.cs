using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Title : TitleImages
    {
        public Title()
        {
        }

        public Title(string name, string description, string principalImageUrl, string posterImageUrl, List<Review> ratings) : base(principalImageUrl, posterImageUrl)
        {
            Name = name;
            Description = description;
            Ratings = ratings;

            if (Ratings.Count() > 0)
            { 
                this.TotalRatings = CalculateTotalRatingsValue();
                this.MedianRate = CalculateMedianRateValue();
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalRatings { get; }

        public decimal MedianRate { get; }

        public List<Review> Ratings { get; set; } = new List<Review>();

        public List<TitleGenre> Genres { get; set; } = new List<TitleGenre>();

        private int CalculateTotalRatingsValue()
        {
            int totalReviews = 0;

            foreach (Review review in Ratings)
            {
                totalReviews += review.ReviewCount;
            }

            return totalReviews;
        }

        private decimal CalculateMedianRateValue()
        { 
            decimal medianRate = 0.0m;

            foreach (Review review in Ratings)
            {
                medianRate += review.Rating;
            }

            return medianRate / Ratings.Count();
        }

        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
