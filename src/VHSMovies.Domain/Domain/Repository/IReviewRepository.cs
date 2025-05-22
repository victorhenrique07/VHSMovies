using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAll();

        Task<Review> GetByTitleExternalId(string titleExternalId);

        Task<List<Review>> GetByReviewerName(string reviewerName);

        Task AddReviews(List<Review> reviews);

        Task SaveChangesAsync();
    }
}
