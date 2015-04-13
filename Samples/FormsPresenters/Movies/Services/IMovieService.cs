using System.Collections.Generic;
using System.Threading.Tasks;
using CoolBeans.Models;

namespace CoolBeans.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> SearchMovie(string movieTitle);
        Task<DetailedMovie> DetailedMovieFromId(int id);
    }
}
