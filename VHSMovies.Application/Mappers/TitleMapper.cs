using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Mappers
{
    public class TitleMapper : AutoMapper.Profile
    {
        public TitleMapper()
        {
            CreateMap<Title, TitleResponse>();
        }
    }
}