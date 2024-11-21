
namespace Models;
public static class FeeCalculator
{
    private static readonly decimal _lateFeePerDay = 0.50M;

    public static decimal? CalculateLateFee(DateTime checkoutDate, DateTime? returnDate, int checkoutDays)
    {
        // Calculate due date
        DateTime dueDate = checkoutDate.AddDays(checkoutDays);

        // Use actual return date or today's date if not returned
        DateTime actualReturnDate = returnDate ?? DateTime.Today;

        // Calculate days late
        int daysLate = (actualReturnDate - dueDate).Days;

        // Return fee or null
        return daysLate > 0 ? daysLate * _lateFeePerDay : null;
    }
}
