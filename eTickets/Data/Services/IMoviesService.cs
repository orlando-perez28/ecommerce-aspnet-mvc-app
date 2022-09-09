using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;

namespace eTickets.Data.Services;

public interface IMoviesService : IEntityBaseRepository<Movie>
{
    Task<Movie> GetMovieByIdAsync(int id);
    Task<MovieDropDownsModel> GetMovieDropDownsValues();
    Task AddNewMovieAsync (MovieViewModel movie);
    Task UpdateMovieAsync (MovieViewModel movie);
}
