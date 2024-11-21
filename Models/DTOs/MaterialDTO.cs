namespace DTOs;

public class MaterialDTO
{
    public string MaterialName { get; set; }
    public int MaterialTypeId { get; set; } // Foreign key reference
    public int GenreId { get; set; }       // Foreign key reference
    public DateTime? OutOfCirculationSince { get; set; }

       // Related DTOs are now optional
    public MaterialTypeDTO? MaterialTypeDTO { get; set; }
    public GenreDTO? GenreDTO { get; set; }

    // Optional collection of related Checkouts
    public List<CheckoutDTO>? Checkouts { get; set; } = new List<CheckoutDTO>();
}
