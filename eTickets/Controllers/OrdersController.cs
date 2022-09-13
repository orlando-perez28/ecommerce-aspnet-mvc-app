using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers;

public class OrdersController : Controller
{
    private readonly IMoviesService _moviesService;
    private readonly ShoppingCart _shoppingCart;
    public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart)
    {
        _moviesService = moviesService;
        _shoppingCart = shoppingCart;
    }

    public IActionResult ShoppingCart()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        var response = new ShoppingCartViewModel();

        _shoppingCart.ShoppingCartItems = items;
        response.ShoppingCart = _shoppingCart;
        response.ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal();

        return View(response);
    }

    public async Task<IActionResult> AddToShoppingCart (int id)
    {
        var movie = await _moviesService.GetMovieByIdAsync(id);

        if (movie != null)
        {
            _shoppingCart.AddItemToCart(movie);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
    {
        var movie = await _moviesService.GetMovieByIdAsync(id);

        if (movie != null)
        {
            _shoppingCart.RemoveItemFromCart(movie);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }
}
