using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models;


public class ShoppingCartItem
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public Movie Movie { get; set; }
    public int Count { get; set; }
    public string ShoppingCartId { get; set; }
}
