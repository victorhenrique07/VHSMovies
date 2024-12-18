using VHSMovies.Infraestructure;
using Microsoft.Extensions.Configuration;

namespace VHSMovies.DataReader
{
    public static class DbContextFactory
    {
        public static DbContextClass Create(IConfiguration configuration)
        {
            return new DbContextClass(configuration);
        }
    }
}