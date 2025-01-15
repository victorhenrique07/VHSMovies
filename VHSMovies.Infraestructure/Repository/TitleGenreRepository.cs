﻿using OpenQA.Selenium.BiDi.Modules.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Infraestructure.Repository
{
    public class TitleGenreRepository : ITitleGenreRepository
    {
        private readonly DbContextClass _dbContext;

        public TitleGenreRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RegisterGenresList(List<TitleGenre> genres)
        {
            try
            {
                await _dbContext.TitlesGenres.AddRangeAsync(genres);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar a entidade: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
                throw;
            }
        }
    }
}