﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllByReviewerName(string reviewerName);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByExternalIdAsync(string externalId);
        Task UpdateAsync(List<T> entity);
        Task RegisterListAsync(List<T> entity);
        Task RegisterAsync(T entity);
        Task SaveChanges();
    }
}
