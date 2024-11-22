namespace DTOs
{
    public class NSSCheckoutDTO
    {

        public int Id { get; set;}
        
        public int MaterialId { get; set; }    // Foreign key reference
        public int PatronId { get; set; }      // Foreign key reference
        public DateTime CheckoutDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public PatronDTO? PatronDTO { get; set; }
        public NSSMaterialDTO? MaterialDTO { get; set; } // Include the NSSMaterialDTO

        public int? DaysOverdue { get; set; } // Optional property for days overdue

    }

}
