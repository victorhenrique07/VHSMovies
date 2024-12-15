﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface ITitleRepository<T> : IRepository<T> where T : Title
    {
        Task<IEnumerable<T>> GetAll(string reviewerName);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByExternalIdAsync(string externalId);
        Task UpdateAsync(T entity);
        Task RegisterAsync(T entity);
    }
}