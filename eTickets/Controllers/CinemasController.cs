using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService _cinemasService;

        public CinemasController(ICinemasService cinemasService)
        {
            _cinemasService = cinemasService;
        }

        public async Task <IActionResult> Index()
        {
            var allCinemas = await _cinemasService.GetAllAsync();
            return View(allCinemas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Cinema cinema)
        {
            //ModelState.Remove("Movies");
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            await _cinemasService.AddAsync(cinema);

            return RedirectToAction(nameof(Index));
        }

        //[HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _cinemasService.GetByIdAsync(id);
            //ModelState.Remove("Actors_Movies");

            if (actor == null)
            {
                return View("NotFound");
            }

            return View(actor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _cinemasService.GetByIdAsync(id);

            if (actor == null)
            {
                return View("NotFound");
            }
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
            ModelState.Remove("Movies");

            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            await _cinemasService.UpdateAsync(id, cinema);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemasService.GetByIdAsync(id);

            if (cinema == null)
            {
                return View("NotFound");
            }
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ModelState.Remove("Movies");

            var cinema = await _cinemasService.GetByIdAsync(id);

            if (cinema == null)
            {
                return View("NotFound");
            }
            await _cinemasService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
