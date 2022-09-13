using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Cart;

public class ShoppingCart
{
    public AppDbContext _appDbContext { get; set; }

    public string ShoppingCartId { get; set; }
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    public ShoppingCart(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public static ShoppingCart GetShoppingCart(IServiceProvider service)
    {
        ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        var context = service.GetService<AppDbContext>();

        string cartId = session.GetString("cartId") ?? Guid.NewGuid().ToString();
        session.SetString("cartId", cartId);

        return new ShoppingCart(context) { ShoppingCartId = cartId };
    }
    public void AddItemToCart(Movie movie)
    {
        var shoppingCartItem = _appDbContext.ShoppingCartItems.FirstOrDefault
            (n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

        if (shoppingCartItem == null)
        {
            shoppingCartItem = new ShoppingCartItem();
            shoppingCartItem.ShoppingCartId = ShoppingCartId;
            shoppingCartItem.Count = 1;
            shoppingCartItem.Movie = movie;

            _appDbContext.Add(shoppingCartItem);
        }
        else
        {
            shoppingCartItem.Count++;
        }
        _appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [EcommerceDB].[dbo].[ShoppingCartItems] OFF");
        _appDbContext.SaveChanges();
        _appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [EcommerceDB].[dbo].[ShoppingCartItems] ON");
    }

    public void RemoveItemFromCart(Movie movie)
    {
        var shoppingCartItem = _appDbContext.ShoppingCartItems.FirstOrDefault
            (n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

        if (shoppingCartItem != null)
        {
            if (shoppingCartItem.Count > 1)
            {
                shoppingCartItem.Count--;
            }
            else
            {
                _appDbContext.Remove(shoppingCartItem);
            }
        }

        _appDbContext.SaveChanges();
    }
    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        return ShoppingCartItems ?? (ShoppingCartItems = _appDbContext.ShoppingCartItems.Where
            (s => s.ShoppingCartId == ShoppingCartId).Include(s => s.Movie).ToList());
    }

    public double GetShoppingCartTotal()
    {
        var total = _appDbContext.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId).
            Select(m => m.Movie.Price * m.Count).Sum();

        return total;
    }
}
