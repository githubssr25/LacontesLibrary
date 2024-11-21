using DTOs;
using Models;


using Microsoft.EntityFrameworkCore;

public class LacontesLibraryDbContext : DbContext
{
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }

    public LacontesLibraryDbContext(DbContextOptions<LacontesLibraryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaterialType>().HasData(
            new MaterialType {Id = 1, Name = "Book", CheckoutDays = 21},
            new MaterialType { Id = 2, Name = "Periodical", CheckoutDays = 7 },
            new MaterialType { Id = 3, Name = "CD", CheckoutDays = 14 },
            new MaterialType { Id = 4, Name = "DVD", CheckoutDays = 14 }
        );
  

                // Seed Genres
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Science Fiction" },
            new Genre { Id = 2, Name = "History" },
            new Genre { Id = 3, Name = "Biography" },
            new Genre { Id = 4, Name = "Mystery" },
            new Genre { Id = 5, Name = "Fantasy" },
            new Genre { Id = 6, Name = "Romance" },
            new Genre { Id = 7, Name = "Horror" }
        );

        // Seed Patrons
        modelBuilder.Entity<Patron>().HasData(
    new Patron { Id = 1, FirstName = "John", LastName = "Doe", Address = "123 Main St", Email = "john.doe@example.com", IsActive = true },
    new Patron { Id = 2, FirstName = "Jane", LastName = "Smith", Address = "456 Elm St", Email = "jane.smith@example.com", IsActive = true },
    new Patron { Id = 3, FirstName = "Emily", LastName = "Johnson", Address = "789 Oak St", Email = "emily.johnson@example.com", IsActive = true },
    new Patron { Id = 4, FirstName = "Michael", LastName = "Brown", Address = "101 Pine St", Email = "michael.brown@example.com", IsActive = true },
    new Patron { Id = 5, FirstName = "Sarah", LastName = "Davis", Address = "321 Cedar St", Email = "sarah.davis@example.com", IsActive = true },
    new Patron { Id = 6, FirstName = "David", LastName = "Wilson", Address = "654 Maple St", Email = "david.wilson@example.com", IsActive = false },
    new Patron { Id = 7, FirstName = "Laura", LastName = "Taylor", Address = "876 Birch Ave", Email = "laura.taylor@example.com", IsActive = false },
    new Patron { Id = 8, FirstName = "Brian", LastName = "Adams", Address = "982 Spruce Blvd", Email = "brian.adams@example.com", IsActive = false }
);

        // Seed Materials
// Seed Materials
modelBuilder.Entity<Material>().HasData(
    new Material { Id = 1, MaterialName = "Dune", MaterialTypeId = 1, GenreId = 1 },
    new Material { Id = 2, MaterialName = "1984", MaterialTypeId = 1, GenreId = 1 },
    new Material { Id = 3, MaterialName = "A Brief History of Time", MaterialTypeId = 1, GenreId = 2 },
    new Material { Id = 4, MaterialName = "Becoming", MaterialTypeId = 1, GenreId = 3 },
    new Material { Id = 5, MaterialName = "Sherlock Holmes", MaterialTypeId = 4, GenreId = 4 },
    new Material { Id = 6, MaterialName = "Harry Potter", MaterialTypeId = 4, GenreId = 5 },
    new Material { Id = 7, MaterialName = "Pride and Prejudice", MaterialTypeId = 2, GenreId = 6 },
    new Material { Id = 8, MaterialName = "The Shining", MaterialTypeId = 3, GenreId = 7 },
    new Material { Id = 9, MaterialName = "The Hobbit", MaterialTypeId = 1, GenreId = 5 },
    new Material { Id = 10, MaterialName = "The Witcher", MaterialTypeId = 4, GenreId = 5 },
    new Material { Id = 11, MaterialName = "The Great Gatsby", MaterialTypeId = 2, GenreId = 6 },
    new Material { Id = 12, MaterialName = "It", MaterialTypeId = 3, GenreId = 7 },
    new Material { Id = 13, MaterialName = "The Martian", MaterialTypeId = 1, GenreId = 1 },
    new Material { Id = 14, MaterialName = "Cosmos", MaterialTypeId = 1, GenreId = 2 },
    new Material { Id = 15, MaterialName = "The Catcher in the Rye", MaterialTypeId = 1, GenreId = 6 },
    new Material { Id = 16, MaterialName = "The Road", MaterialTypeId = 1, GenreId = 1 },
    new Material { Id = 17, MaterialName = "Sapiens", MaterialTypeId = 1, GenreId = 2 }
);


modelBuilder.Entity<Checkout>().HasData(
    // Overdue checkouts
    new Checkout { Id = 7, MaterialId = 1, PatronId = 1, CheckoutDate = DateTime.Today.AddDays(-30), ReturnDate = null }, // Dune (9 days late)
    new Checkout { Id = 8, MaterialId = 2, PatronId = 2, CheckoutDate = DateTime.Today.AddDays(-50), ReturnDate = null }, // 1984 (29 days late)
    new Checkout { Id = 9, MaterialId = 6, PatronId = 3, CheckoutDate = DateTime.Today.AddDays(-25), ReturnDate = null }, // Harry Potter (11 days late)
    new Checkout { Id = 10, MaterialId = 10, PatronId = 4, CheckoutDate = DateTime.Today.AddDays(-40), ReturnDate = null }, // The Witcher (26 days late)
    new Checkout { Id = 11, MaterialId = 8, PatronId = 5, CheckoutDate = DateTime.Today.AddDays(-20), ReturnDate = null }  // The Shining (6 days late)
);




//THIS IS IMPORTANT 
modelBuilder.Entity<Material>()
.HasOne(m => m.Genre)
.WithMany()
.HasForeignKey(m => m.GenreId);
//In your example, .WithMany() does not specify the navigation property on the Genre side because the 
//Genre class does not define a navigation property for Material. If it did, the relationship might look like this:
// modelBuilder.Entity<Material>()
//     .HasOne(m => m.Genre)
//     .WithMany(g => g.Materials) // Assuming `Genre` has a `List<Material> Materials` property
//     .HasForeignKey(m => m.GenreId);
// ONE TO MANY RELATIONSHIP 

modelBuilder.Entity<Material>()
    .HasOne(m => m.MaterialType)
    .WithMany()
    .HasForeignKey(m => m.MaterialTypeId);

// KEY DISTINCTION THIS IS A 1 TO 1 RELATIONSIHP IF WE DID IT LIKE THIS 
//  .HasForeignKey<Material>(m => m.CheckoutId); YOU MUST SPECIFY FOREIGN KEY LIKE THIS 
// modelBuilder.Entity<Material>()
//     .HasOne(m => m.Checkout)
//     .WithOne()
//     .HasForeignKey<Material>(m => m.CheckoutId);
//however we are giong to still make material to checkout a 1 to many relationsihp 
//VERY VERY IMPORTANT KEY POINT IS THAT IN THE GENRE IF IT IS 1 GENRE MANY MATERIALS 
// YOU AHVE TO SPECIFY THE FOREIGN KEY IN AMTERIALS BUT YOU DO NOT HAVE TO SPECIFY ANY FOREIGN KEY RELATIONSHIP
// WITHIN GENRES. THIS IS APPARENLTY V COMMON IN DBS AND HOW APPS ARE SET UP. YOU CAN LEAVE THAT SIDE BLANK
// AND THE DEFAULT ASSUMPTION UNLESS YOU HAVE A FOREIGN KEY CONSTRAINT IN THE DB(IE THE GERNE ID FK MUST BE UNIQUE
// IT CANNOT BE REPEATED SO 1 MATERIAL CAN ONLY LINK 1 GENRE THEN THE ASSUMPTION IS WITHOUT THAT JUST A 1 TO MANY RELATIONSHIP)

// THE THING THOUGH IS FOR FLUENT API YOU STILL DO HAVE TO AND WANT TO SPECIFY THIS like we do right below
// basially saying that 1 checkout will have a foreign key explicitly of mateiralId or material will have foreign key reference for genreId
// basically explicitly saying 1 material can have many checkouts or genres 


// Material to Checkout (One-to-Many)
modelBuilder.Entity<Material>()
    .HasMany(m => m.Checkouts) // A Material can have many Checkouts
    .WithOne(c => c.Material) // Each Checkout is for one Material
    .HasForeignKey(c => c.MaterialId); // The foreign key is in the Checkout table

modelBuilder.Entity<Material>()
.HasOne(m => m.MaterialType) // Each Material has one MaterialType
.WithMany()  // MaterialType does not reference Material
.HasForeignKey(m => m.MaterialTypeId); // Foreign key in Material table



// Seed Checkouts
 
modelBuilder.Entity<Checkout>().HasData(
    new Checkout { Id = 1, MaterialId = 13, PatronId = 1, CheckoutDate = new DateTime(2024, 1, 1), ReturnDate = null },
    new Checkout { Id = 2, MaterialId = 2, PatronId = 2, CheckoutDate = new DateTime(2024, 2, 1), ReturnDate = new DateTime(2024, 7, 2) },
    new Checkout { Id = 3, MaterialId = 15, PatronId = 3, CheckoutDate = new DateTime(2024, 3, 1), ReturnDate = null },
    new Checkout { Id = 4, MaterialId = 17, PatronId = 4, CheckoutDate = new DateTime(2024, 4, 1), ReturnDate = new DateTime(2024, 5, 1) },
    new Checkout { Id = 5, MaterialId = 5, PatronId = 1, CheckoutDate = new DateTime(2024, 5, 1), ReturnDate = null },
    new Checkout { Id = 6, MaterialId = 16, PatronId = 2, CheckoutDate = new DateTime(2024, 6, 1), ReturnDate = null }
);
    
modelBuilder.Entity<Checkout>()
    .HasOne(c => c.Patron) // Each Checkout is associated with one Patron
    .WithMany(p => p.Checkouts) // One Patron can have many Checkouts
    .HasForeignKey(c => c.PatronId); // The foreign key is in the Checkout table

    }




}