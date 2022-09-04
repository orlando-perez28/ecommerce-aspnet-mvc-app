using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersService _producersService;

        public ProducersController(IProducersService producersService)
        {
            _producersService = producersService;
        }

        public async Task<IActionResult> Index()
        {
            var allProducers = await _producersService.GetAllAsync();
            return View(allProducers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var producer = await _producersService.GetByIdAsync(id);

            if (producer == null)
            {
                return View("NotFound");
            }
            return View(producer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Producer producer)
        {
            ModelState.Remove("Movies");

            if (!ModelState.IsValid)
                return View(producer);

            await _producersService.AddAsync(producer); 
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _producersService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Producer producer)
        {
            ModelState.Remove("Movies");

            if (!ModelState.IsValid)
                return View(producer);

            if (id == producer.Id)
            {

                await _producersService.UpdateAsync(id, producer);
                return RedirectToAction(nameof(Index));
            }

            return View(producer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _producersService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _producersService.GetByIdAsync(id);

            if (result == null)
                return View("NotFound");

            await _producersService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
