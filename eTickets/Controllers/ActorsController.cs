using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService _appDbContext;

        public ActorsController(IActorsService appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _appDbContext.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")]Actor actor)
        {
            ModelState.Remove("Actors_Movies");
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _appDbContext.AddAsync(actor);

            return RedirectToAction(nameof(Index));
        }

        //[HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _appDbContext.GetByIdAsync(id);
            //ModelState.Remove("Actors_Movies");
            
            if (actor == null)
            {
                return View("NotFound");
            }

            return View(actor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _appDbContext.GetByIdAsync(id);
            
            if (actor == null)
            {
                return View("NotFound");
            }
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Actor actor)
        {
            ModelState.Remove("Actors_Movies");
            
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _appDbContext.UpdateAsync(id, actor);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _appDbContext.GetByIdAsync(id);

            if (actor == null)
            {
                return View("NotFound");
            }
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ModelState.Remove("Actors_Movies");

            var actor = await _appDbContext.GetByIdAsync(id);

            if (actor == null)
            {
                return View("NotFound");
            }
            await _appDbContext.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }  
}
