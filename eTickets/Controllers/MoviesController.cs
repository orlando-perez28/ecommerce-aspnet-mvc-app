using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _moviesService;

        public MoviesController(IMoviesService appDbContext)
        {
            _moviesService = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var allMovies = await _moviesService.GetAllAsync(m => m.Cinema);
                //Include(c => c.Cinema).OrderBy(m => m.Name).ToListAsync();
            return View(allMovies);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _moviesService.GetAllAsync(m => m.Cinema);
            //Include(c => c.Cinema).OrderBy(m => m.Name).ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                var filtered = allMovies.Where(m => m.Name.Contains(searchString) || m.Description.Contains(searchString)).ToList();   
                return View("Index",filtered);
            }

            return View("Index", allMovies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _moviesService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return View("NotFound");
            }
            return View(movie);
        }

        public async Task<IActionResult> Create()
        {
            var movieDropDowns = await _moviesService.GetMovieDropDownsValues();

            ViewBag.CinemaId = new SelectList(movieDropDowns.Cinemas, "Id", "Name");
            ViewBag.ActorIds = new SelectList(movieDropDowns.Actors, "Id", "FullName");
            ViewBag.ProducerId = new SelectList(movieDropDowns.Producers, "Id", "FullName");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropDowns = await _moviesService.GetMovieDropDownsValues();

                ViewBag.CinemaId = new SelectList(movieDropDowns.Cinemas, "Id", "Name");
                ViewBag.ActorIds = new SelectList(movieDropDowns.Actors, "Id", "FullName");
                ViewBag.ProducerId = new SelectList(movieDropDowns.Producers, "Id", "FullName");
                return View(movie);
            }
     

            await _moviesService.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _moviesService.GetMovieByIdAsync(id);

            if (result == null)
                return View("NotFound");

            var response = new MovieViewModel();
            response.Id = id;
            response.Price = result.Price;
            response.MovieCategory = result.MovieCategory;
            response.ProducerId = result.ProducerId;    
            response.ImageURL = result.ImageURL;
            response.CinemaId = result.CinemaId;
            response.ActorsIds = result.Actors_Movies.Select(am => am.ActorId).ToList();
            response.Name = result.Name;
            response.Description = result.Description;
            response.StartDate = result.StartDate;
            response.EndDate = result.EndDate;

            var movieDropDowns = await _moviesService.GetMovieDropDownsValues();

            ViewBag.CinemaId = new SelectList(movieDropDowns.Cinemas, "Id", "Name");
            ViewBag.ActorIds = new SelectList(movieDropDowns.Actors, "Id", "FullName");
            ViewBag.ProducerId = new SelectList(movieDropDowns.Producers, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieViewModel movie)
        {
            //ModelState.Remove("Movies");

            if (id != movie.Id)
                return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropDowns = await _moviesService.GetMovieDropDownsValues();

                ViewBag.CinemaId = new SelectList(movieDropDowns.Cinemas, "Id", "Name");
                ViewBag.ActorIds = new SelectList(movieDropDowns.Actors, "Id", "FullName");
                ViewBag.ProducerId = new SelectList(movieDropDowns.Producers, "Id", "FullName");               
            }

            await _moviesService.UpdateMovieAsync( movie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _moviesService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _moviesService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            await _moviesService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
