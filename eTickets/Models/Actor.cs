using System.ComponentModel.DataAnnotations;

namespace eTickets.Models;

public class Actor
{
    [Key]
    public int Id { get; set; }
    
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "FullName is Required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "FullName must be between 3 and 50")]
    public string FullName { get; set; }

    [Display(Name = "Profile Picture")]
    [Required(ErrorMessage = "ProfilePictureURL is Required")]    
    public string ProfilePictureURL { get; set; }

    [Display(Name = "Biography")]
    [Required(ErrorMessage = "Biography is Required")]
    public string Bio { get; set; }

    public List<Actor_Movie> Actors_Movies { get; set; }
}
