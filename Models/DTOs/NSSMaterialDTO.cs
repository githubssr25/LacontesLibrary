namespace DTOs;

public class NSSMaterialDTO
{

    public int Id {get ; set;}
    public string MaterialName { get; set; }
    public int MaterialTypeId { get; set; } // Foreign key reference

    public MaterialTypeDTO MaterialTypeDTO { get; set;}
    public int GenreId { get; set; }       // Foreign key reference
    public DateTime? OutOfCirculationSince { get; set; }

}
