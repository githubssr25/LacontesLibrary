using System.ComponentModel.DataAnnotations;

namespace DTOs;
public class CreateMaterialDTO
{
    [Required]
    public string MaterialName { get; set; } = string.Empty;

    [Required]
    public int MaterialTypeId { get; set; } // Foreign key reference to MaterialType

    [Required]
    public int GenreId { get; set; } // Foreign key reference to Genre

    public DateTime? OutOfCirculationSince { get; set; } // Optional field for out-of-circulation materials
}
