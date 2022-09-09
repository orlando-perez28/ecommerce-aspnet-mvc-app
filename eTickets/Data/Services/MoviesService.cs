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

    public async Task AddNewMovieAsync(MovieViewModel movie)
    {
        var newMovie = new Movie();
        newMovie.Name = movie.Name;
        newMovie.Price = movie.Price;
        newMovie.Description = movie.Description;
        newMovie.MovieCategory = movie.MovieCategory;
        newMovie.ImageURL = movie.ImageURL;
        newMovie.CinemaId = movie.CinemaId;
        newMovie.ProducerId = movie.ProducerId;
        newMovie.StartDate = movie.StartDate;
        newMovie.EndDate = movie.EndDate;

        await _appDbContext.Movies.AddAsync(newMovie);
        await _appDbContext.SaveChangesAsync();

        foreach (var item in movie.ActorsIds)
        {
            var newActorMovie = new Actor_Movie();
            newActorMovie.MovieId = newMovie.Id;
            newActorMovie.ActorId = item;

            await _appDbContext.Actors_Movies.AddAsync(newActorMovie);
        }
        await _appDbContext.SaveChangesAsync();
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

    public async Task UpdateMovieAsync(MovieViewModel movie)
    {
        var dbMovie = await _appDbContext.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);    

        if (dbMovie != null)
        {            
            dbMovie.Name = movie.Name;
            dbMovie.Price = movie.Price;
            dbMovie.Description = movie.Description;
            dbMovie.MovieCategory = movie.MovieCategory;
            dbMovie.ImageURL = movie.ImageURL;
            dbMovie.CinemaId = movie.CinemaId;
            dbMovie.ProducerId = movie.ProducerId;
            dbMovie.StartDate = movie.StartDate;
            dbMovie.EndDate = movie.EndDate;

             await _appDbContext.SaveChangesAsync();
        }

        var existingActorDB = _appDbContext.Actors_Movies.Where(a => a.MovieId == movie.Id).ToList();
        _appDbContext.Actors_Movies.RemoveRange(existingActorDB);
        await _appDbContext.SaveChangesAsync();

        foreach (var item in movie.ActorsIds)
        {
            var newActorMovie = new Actor_Movie();
            newActorMovie.MovieId = movie.Id;
            newActorMovie.ActorId = item;

            await _appDbContext.Actors_Movies.AddAsync(newActorMovie);
        }
        await _appDbContext.SaveChangesAsync();
    }
}
