
using System.ComponentModel.DataAnnotations;

namespace Models;

public class MaterialType
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CheckoutDays { get; set; }
}
