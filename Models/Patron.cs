using System.ComponentModel.DataAnnotations;

namespace Models;

public class Patron
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

     // Navigation property for the one-to-many relationship with Checkouts
    public ICollection<Checkout > Checkouts { get; set; } = new List<Checkout>();
  public decimal Balance // Total unpaid late fees
{
    get
    {
        decimal totalBalance = 0;

        // Process checkouts to calculate total balance
        foreach (var checkout in Checkouts)
        {
            if (checkout.LateFee.HasValue)
            {
                Console.WriteLine($"DEBUG: Checkout ID {checkout.Id}, LateFee: {checkout.LateFee.Value}");
            }
            else
            {
                Console.WriteLine($"DEBUG: Checkout ID {checkout.Id} has no LateFee.");
            }

            if (checkout.LateFee.HasValue && checkout.LateFee.Value > 0)
            {
                totalBalance += checkout.LateFee.Value;
            }
        }

        Console.WriteLine($"DEBUG: Total balance for Patron ID {Id}, Name: {FirstName} {LastName} is {totalBalance}");
        return totalBalance;
    }
}


    // Check if all fees are paid dynamically
    public bool AllFeesPaid
    {
        get
        {
            return Checkouts
                .Where(checkout => checkout.LateFee.HasValue)
                .All(checkout => checkout.LateFee.Value == 0);
        }
    }
}
