using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string reviewerName);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByExternalIdAsync(string externalId);
        Task UpdateByExternalIdAsync(T entity);
        Task RegisterAsync(T entity);
    }
}
