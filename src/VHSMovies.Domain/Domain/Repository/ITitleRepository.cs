using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface ITitleRepository
    {
        Task<IEnumerable<Title>> GetAll();
        Task<IEnumerable<Title>> GetAllByReviewerName(string reviewerName);
        Task<IEnumerable<Title>> GetAllByGenreId(int genreId);
        Task<Title> GetByIdAsync(int id);
        Task<Title> GetByExternalIdAsync(string externalId);
        Task UpdateAsync(List<Title> entity);
        Task RegisterAsync(Title entity);
        Task RegisterListAsync(List<Title> entity);

        IQueryable<Title> Query();
        Task SaveChangesAsync();
    }
}
