using eTickets.Data.Base;
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
}
