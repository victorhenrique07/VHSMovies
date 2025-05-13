using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetAll();
        Task<T> GetByIdAsync(int id);
        Task RegisterListAsync(IReadOnlyCollection<T> entity);
        Task RegisterAsync(T entity);
        Task SaveChanges();
    }
}
