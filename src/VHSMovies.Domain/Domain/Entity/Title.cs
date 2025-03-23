using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Title(string name, DateOnly? releaseDate, string description, 
            string principalImageUrl, string posterImageUrl, List<Review> ratings) :
            base(principalImageUrl, posterImageUrl)
        {
            Name = name;
            ReleaseDate = releaseDate;
            Description = description;
            Ratings = ratings;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public string Description { get; set; }

        public int TotalReviews {
            get
            {
                return CalculateTotalRatingsValue();
            }
        }

        public decimal AverageRating { 
            get 
            {
                return CalculateMedianRateValue();
            } 
        }

        public decimal Relevance
        {
            get
            {
                return CalculateRelevance();
            }
        }

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

        public decimal CalculateRelevance()
        {
            decimal totalRatingsLog = (decimal)Math.Log10(this.TotalReviews);

            return (this.AverageRating * 0.6m) + totalRatingsLog;
        }

        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
