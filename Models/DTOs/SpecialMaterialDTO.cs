namespace DTOs;

public class SpecialMaterialDTO
{
    public string MaterialName { get; set; }
    public int MaterialTypeId { get; set; } // Foreign key reference
    public int GenreId { get; set; }       // Foreign key reference
    public DateTime? OutOfCirculationSince { get; set; }

    public MaterialTypeDTO MaterialTypeDTO { get; set; }
    public GenreDTO GenreDTO { get; set; }
    public List<CheckoutDTO> Checkouts { get; set; } = new List<CheckoutDTO>();
}
