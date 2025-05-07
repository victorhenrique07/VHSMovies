using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Title
    {
        public Title()
        {
        }

        public Title(string name, TitleType type, int? releaseDate, string imdbId)
        {
            Name = name;
            Type = type;
            ReleaseDate = releaseDate;
            IMDB_Id = imdbId;
        }

        public int Id { get; set; }

        public string IMDB_Id { get; set; }

        public TitleType Type { get; set; }

        public string Name { get; set; }

        public int Runtime { get; set; }

        public int? ReleaseDate { get; set; }

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

        public decimal Relevance { get; set; } = 0m;

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

        public decimal SetRelevance()
        {
            decimal totalRatingsLog = this.TotalReviews > 0
                ? (decimal)Math.Log10(this.TotalReviews)
                : 0m;

            decimal averageWeight = 0.5m;
            decimal reviewsWeight = 1.0m;

            return (this.AverageRating * averageWeight) + (totalRatingsLog * reviewsWeight);
        }

        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
