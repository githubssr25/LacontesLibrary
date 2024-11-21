
using  Models;
using DTOs;

public class CheckoutWithLateFeeDto
{
    private static readonly decimal _lateFeePerDay = 0.50M; // Late fee per day

    public int Id { get; set; }
    public int MaterialId { get; set; }    // Foreign key reference
    public int PatronId { get; set; }      // Foreign key reference
    public DateTime CheckoutDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public PatronDTO? PatronDTO { get; set; }
    public NSSMaterialDTO? MaterialDTO { get; set; }

    // Calculated property for LateFee
    public decimal? LateFee
    {
        get
        {
            if (MaterialDTO?.MaterialTypeDTO == null) return null;

            // Calculate due date
            return FeeCalculator.CalculateLateFee(CheckoutDate, ReturnDate, MaterialDTO.MaterialTypeDTO.CheckoutDays);
        }
    }
}
