using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services;

public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
{
    private readonly AppDbContext _appDbContext;
    public MoviesService(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        var movie = await _appDbContext.Movies.Include(c => c.Cinema).
            Include(p => p.Producer).
            Include(am => am.Actors_Movies).
            ThenInclude(a => a.Actor).FirstOrDefaultAsync(i => i.Id == id);

        return movie;
    }

    public async Task<MovieDropDownsModel> GetMovieDropDownsValues()
    {
        var response = new MovieDropDownsModel();
        response.Actors = await _appDbContext.Actors.OrderBy(a => a.FullName).ToListAsync();
        response.Producers = await _appDbContext.Producers.OrderBy(p => p.FullName).ToListAsync();
        response.Cinemas = await _appDbContext.Cinemas.OrderBy(c => c.Name).ToListAsync();

        return response;
    }
}
