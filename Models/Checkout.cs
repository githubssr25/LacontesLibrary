using System.ComponentModel.DataAnnotations;

using Models;
using DTOs;

public class Checkout
{
    public int Id { get; set; }

    [Required]
    public int MaterialId { get; set; }

    [Required]
    public int PatronId { get; set; }

    [Required]
    public DateTime CheckoutDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    // Navigation properties
    public Material Material { get; set; }
    public Patron Patron { get; set; }

    public decimal? LateFee
    {
        get{
            if(Material?.MaterialType == null) return null;
            return FeeCalculator.CalculateLateFee(CheckoutDate, ReturnDate, Material.MaterialType.CheckoutDays);
        }
    }
 // The LateFee property in your code is a calculated property, which means it is not stored in the database. 
 //Instead, its value is computed dynamically whenever the property is accessed.  
 //IMPORTANT: LateFee doesn't exist as a column in the database. 
 //he result is dynamically computed based on the current state of the object's properties.
 //Since LateFee is not part of the database schema, its value is not stored in the database or retrieved during a query.
    

}


// //goign to take this out 

//           public bool? Paid
//     {
//         get
//         {
//             // Only meaningful if LateFee exists
//             if (LateFee == null) return null;
//             return LateFee == 0; // Automatically paid if no fee remains
// //LateFee becomes 0 (e.g., no late fee is owed), the getter will return true for Paid because of this line:
// // How return LateFee == 0 Works
// // The key here is LateFee is a nullable decimal (decimal?), and the expression LateFee == 0 is a Boolean comparison.
// // LateFee == 0 evaluates to:
// // true if LateFee has a value and that value is 0.
// // false if LateFee has a value that is not 0.
// //and if its null never reachs this pt due to  if (LateFee == null) return null;
// //return LateFee == 0; does not return an int; it performs a comparison that results in a Boolean value     
//  // IF LATEFEE > 0 THIS WILL RETURN TO FALSE       
       
//         }
//         set
//         {

//             //This means that if LateFee is null (indicating there's no fee or it's not applicable), any attempt to set Paid will be
//             // ignored because the condition LateFee != null is false. Only when there is a LateFee (i.e., it is not null) will the provided value for Paid (true or false) be stored in _manualPaid
//             if (LateFee != null)
//             {
//                 // Allow manual override if LateFee exists
//                 // for C# set, value is implicit property you dont need to define it IMPORTANT
//                 //it represents the value that is being assigned to the property by the code using it.
// //checkout.Paid = true;), that value is automatically passed to the value parameter inside the set accessor.
// // THIS MEANS PAID WILL ALWAYS BE INITIALLY NULL SOMEWHERE IN TEH CODE HAS TO SET THE VALUE TO TRUE OR FALSE

//                 _manualPaid = value;
//             }
//         }
//     }
// // OK I WAS MISTAKEN BASED OFF THIS LOGIC AS LONG AS LATEFEE IS A CALCULATED PROPERTY THAT IS AUTOMATICALLY DETERMINED 
// // THEN THE LATEFEE WONT BE NULL AND IF ITS NOT NULL THEN PAID WILL RETURN TRUE OR FALSE BASED OFF     return LateFee == 0;
// //HOWEVER FOR THE SETTER YOU CAN STILL OVERRIDE AND SET PAID TO WHATEVER YOU WANT IN TEH CODE IF YOU WANT 



//     private bool? _manualPaid; // Backing field for manual override// Indicates if the fee for this checkout has been paid

// //_manualPaid acts as a backing field for manual overrides, storing the explicitly set value for Paid (true/false) when relevant.
// // Without _manualPaid, you wouldn’t have a way to distinguish between:
// // Automatically determining "Paid" based on LateFee.
// // Manually overriding "Paid" for edge cases.

