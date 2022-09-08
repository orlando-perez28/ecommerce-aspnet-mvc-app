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
        public async Task<IActionResult> Create([Bind("Price,Name,Description,ImageURL")] Movie movie)
        {
            ModelState.Remove("Actor_Movie");

            if (!ModelState.IsValid)
                return View(movie);

            await _moviesService.AddAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _moviesService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Name,Description,ImageURL")] Movie movie)
        {
            ModelState.Remove("Movies");

            if (!ModelState.IsValid)
                return View(movie);

            if (id == movie.Id)
            {

                await _moviesService.UpdateAsync(id, movie);
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
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
