using eTickets.Data.Base;
using eTickets.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models;

public class MovieViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Price is required")]
    [Display(Name = "Price in $")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name of movie")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Movie Description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "ImageURL is required")]
    [Display(Name = "Movie Poster URL")]
    public string ImageURL { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    [Display(Name = "StartDate")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    [Display(Name = "EndDate")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "MovieCategory is required")]
    [Display(Name = "Select a Movie Category")]
    public MovieCategory MovieCategory { get; set; }

    [Required(ErrorMessage = "ActorsIds is required")]
    [Display(Name = "Select an ActorsIds")]
    public List<int> ActorsIds { get; set; }

    [Required(ErrorMessage = "CinemaId is required")]
    [Display(Name = "Select a CinemaId")]
    public int CinemaId { get; set; }

    [Required(ErrorMessage = "ProducerId is required")]
    [Display(Name = "Select a ProducerId")]
    public int ProducerId { get; set; }
}
