using System.ComponentModel.DataAnnotations;

namespace Models;

public class Material
{
    public int Id { get; set; }

    [Required]
    public string MaterialName { get; set; } = string.Empty;

    [Required]
    public int MaterialTypeId { get; set; }

    [Required]
    public int GenreId { get; set; }

    public DateTime? OutOfCirculationSince { get; set; }

    // Navigation properties
    public MaterialType MaterialType { get; set; }
    public Genre Genre { get; set; }

        // Optional navigation to a single Checkout
   public ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>(); // Updated
}
