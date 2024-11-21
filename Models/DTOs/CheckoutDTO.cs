
namespace DTOs;
public class CheckoutDTO
{
    public int MaterialId { get; set; }    // Foreign key reference
    public int PatronId { get; set; }      // Foreign key reference
    public DateTime CheckoutDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public PatronDTO? PatronDTO { get; set;}
}


