using System.ComponentModel.DataAnnotations;

namespace DTOs;
public class CreateCheckoutDTO
{
     public int MaterialId { get; set; }    // Foreign key reference
    public int PatronId { get; set; }      // Foreign key reference
    public DateTime? CheckoutDate { get; set; }
    public DateTime? ReturnDate { get; set; }


}