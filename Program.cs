using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Models;
using DTOs;
using AutoMapper;
using Mapper;

var builder = WebApplication.CreateBuilder(args);

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Allows our API endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LacontesLibraryDbContext>(builder.Configuration["LacontesLibraryDbConnectionString"]);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper
// Add services to the container.

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/materials", (LacontesLibraryDbContext db) =>
{
 var materialDTOs = db.Materials
 .Where(material => material.OutOfCirculationSince == null)
 .Select(eachMaterial => new MaterialDTO
 { MaterialName = eachMaterial.MaterialName,
   MaterialTypeId = eachMaterial.MaterialTypeId,
   GenreId = eachMaterial.GenreId,
   OutOfCirculationSince = eachMaterial.OutOfCirculationSince,
   MaterialTypeDTO = db.MaterialTypes.
   Where(materialType => materialType.Id == eachMaterial.MaterialTypeId)
   .Select( materialTypeVar => new MaterialTypeDTO
   {Name = materialTypeVar.Name,
    CheckoutDays = materialTypeVar.CheckoutDays
   }).FirstOrDefault(),
   GenreDTO = db.Genres.
   Where(eachGenre => eachGenre.Id == eachMaterial.GenreId)
   .Select(eachGenreVar => new GenreDTO
   { Name = eachGenreVar.Name
   }).FirstOrDefault()
 }).ToList();

return Results.Ok(materialDTOs);
});


// to test this one you need query string parameters not path variables 
// http://localhost:5167/api/materials?materialTypeId=1&genreId=2
app.MapGet("/api/materialsQueryParams", (LacontesLibraryDbContext db, int? materialTypeId, int? genreId) =>
{
var materialDTOs = db.Materials
    .Where(materialForFilter =>
        (materialTypeId.HasValue || genreId.HasValue) && // Ensure at least one is provided
        (!materialTypeId.HasValue || materialForFilter.MaterialTypeId == materialTypeId) &&
        (!genreId.HasValue || materialForFilter.GenreId == genreId)
    )
    .Select(materialForDTO => new MaterialDTO
    {
        MaterialName = materialForDTO.MaterialName,
        MaterialTypeId = materialForDTO.MaterialTypeId,
        GenreId = materialForDTO.GenreId,
        OutOfCirculationSince = materialForDTO.OutOfCirculationSince,
        MaterialTypeDTO = db.MaterialTypes
            .Where(materialType => materialType.Id == materialForDTO.MaterialTypeId)
            .Select(materialTypeVar => new MaterialTypeDTO
            {
                Name = materialTypeVar.Name,
                CheckoutDays = materialTypeVar.CheckoutDays
            }).FirstOrDefault() ?? new MaterialTypeDTO { Name="unknown no materialTypeDTO was found", CheckoutDays = 0},
        GenreDTO = db.Genres
            .Where(genre => genre.Id == materialForDTO.GenreId)
            .Select(genreVar => new GenreDTO
            {
                Name = genreVar.Name
            }).FirstOrDefault() ?? new GenreDTO { Name = "Unknown no GenreDTO was found"}
    })
    .ToList();


    return Results.Ok(materialDTOs);
});

//The librarians would like to see details for a material. Include the Genre, MaterialType, 
//and Checkouts (as well as the Patron associated with each checkout using ThenInclude).
// Do not add the Material and MaterialType to each checkout.
app.MapGet("/api/materials/special/{materialId}", (LacontesLibraryDbContext db, int materialId, IMapper mapper) =>
{

// var material = db.Materials
//     .Include(material => material.Checkouts)
//     .ThenInclude(checkout => checkout.Patron)
//     .FirstOrDefault(m => m.Id == materialId);

// var checkoutsForMaterial = db.Checkouts
//     .Include(c => c.Patron)
//     .Where(c => c.MaterialId == materialId)
//     .ToList();

// Console.WriteLine($"Checkouts for Material ID {materialId}: {checkoutsForMaterial.Count}");
// foreach (var checkout in checkoutsForMaterial)
// {
//     Console.WriteLine($"Checkout ID: {checkout.Id}, Patron ID: {checkout.PatronId}");
// }

// Console.WriteLine($"Material Retrieved: {material?.MaterialName ?? "NULL"}");
// Console.WriteLine($"Checkouts Count: {material?.Checkouts.Count ?? 0}");
// foreach (var checkout in material?.Checkouts ?? new List<Checkout>())
// {
//     Console.WriteLine($"Checkout ID: {checkout.Id}, Patron ID: {checkout.PatronId}");
// }

    var materialDTO = db.Materials
    .Include(material => material.MaterialType)
    .Include(material => material.Genre)
    .Include(material => material.Checkouts)
    .ThenInclude(checkout => checkout.Patron)
    .Select(ourOverallDTO => new SpecialMaterialDTO
    {MaterialName = ourOverallDTO.MaterialName,
    MaterialTypeId = ourOverallDTO.MaterialTypeId,
    GenreId = ourOverallDTO.GenreId,
    OutOfCirculationSince = ourOverallDTO.OutOfCirculationSince,
    MaterialTypeDTO = mapper.Map<MaterialTypeDTO>(ourOverallDTO.MaterialType),
    GenreDTO = mapper.Map<GenreDTO>(ourOverallDTO.Genre),
    Checkouts = mapper.Map<List<CheckoutDTO>>(ourOverallDTO.Checkouts)
    })
    .FirstOrDefault();
    // Ensure a single material is retrieved

   // Log the Checkouts in the DTO
    Console.WriteLine($"Checkouts Count in DTO: {materialDTO?.Checkouts.Count ?? 0}");
    // foreach (var checkoutDTO in materialDTO?.Checkouts ?? new List<CheckoutDTO>())
    // {
    //     Console.WriteLine($"CheckoutDTO ID: {checkoutDTO.MaterialId}, PatronDTO Name: {checkoutDTO.PatronDTO?.FirstName ?? "NULL"}");
    // }

return Results.Ok(materialDTO);
});

app.MapPost("/api/materials", async (LacontesLibraryDbContext db, IMapper mapper, CreateMaterialDTO createMaterialDTO) =>
{
    try {
    var ourMaterial = mapper.Map<Material>(createMaterialDTO);
    db.Materials.Add(ourMaterial);
    await db.SaveChangesAsync(); // Save to establish relationships

    var materialWithNavProp = db.Materials
                       .Include(material => material.Genre)
                      .Include(material => material.MaterialType)
                      .FirstOrDefault(material => material.Id == ourMaterial.Id);

        // var materialDTO = mapper.Map<MaterialDTO>(ourMaterial);

    var materialDTO = mapper.Map<MaterialDTO>(materialWithNavProp);


        return Results.Created($"/api/materials/{ourMaterial.Id}", materialDTO);
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred while adding the material.");
    }
});

app.MapPut("/api/materials/{id}/remove", async (LacontesLibraryDbContext db, IMapper mapper, int id) =>
{
    // Load the material for update
    var ourMaterial = await db.Materials.SingleOrDefaultAsync(material => material.Id == id);

    if (ourMaterial == null)
    {
        return Results.NotFound($"Material with ID {id} not found.");
    }

    // Update the property
    ourMaterial.OutOfCirculationSince = DateTime.Now;

    // Save the changes
    await db.SaveChangesAsync();

    var materialWithNavProp = db.Materials
                       .Include(material => material.Genre)
                      .Include(material => material.MaterialType)
                      .FirstOrDefault(material => material.Id == ourMaterial.Id);

    var materialDTO = mapper.Map<MaterialDTO>(materialWithNavProp);

    return Results.Ok(materialDTO);

});

app.MapGet("/api/materialtypes", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var materialTypeDTOs = db.MaterialTypes
        .Select(mt => mapper.Map<MaterialTypeDTO>(mt))
        .ToList();

    return Results.Ok(materialTypeDTOs);
});



app.MapGet("/api/genres", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var genreDTO = db.Genres
    .Select(genre => mapper.Map<GenreDTO>(genre))
     .ToList();

    return Results.Ok(genreDTO);
});

//below also could work 
//var genreDTOs = db.Genres
//     .ProjectTo<GenreDTO>(mapper.ConfigurationProvider)
//     .ToList();
// return Results.Ok(genreDTOs);




app.MapGet("api/patrons/materials", (LacontesLibraryDbContext db, IMapper mapper) => {

var patronsWithCheckouts = db.Patrons
    .Include(p => p.Checkouts) // Load Checkouts with Patron
    .ThenInclude(checkouts => checkouts.Material)
    .ThenInclude(material => material.MaterialType)
    .ToList();

//note without slect could do something like this   CheckoutDTOs = mapper.Map<List<CheckoutDTO>>(patron.Checkouts) /

var ourPatronDTO = patronsWithCheckouts.Select(patron => 
    new {
        PatronDTO = mapper.Map<PatronDTO>(patron),
        //THIS IS WRONG THIS PART 
        CheckoutDTO = patron.Checkouts
        .Select(checkout => new 
        { CheckoutDTO = mapper.Map<CheckoutDTO>(checkout),
          MaterialDTO = mapper.Map<MaterialDTO>(checkout.Material),
          MaterialTypeDTO = mapper.Map<MaterialTypeDTO>(checkout.Material.MaterialType)
        }
        ).ToList()
    }
).ToList();

return Results.Ok(ourPatronDTO);

});

app.MapPut("/api/patrons/{id}", (LacontesLibraryDbContext db, int id, PatronDTO updatedPatronDTO) =>
{
    // Retrieve the patron from the database
    var existingPatron = db.Patrons.Find(id);

    if (existingPatron == null)
    {
        return Results.NotFound($"Patron with ID {id} not found.");
    }

    // Update properties
    existingPatron.Address = updatedPatronDTO.Address ?? existingPatron.Address;
    existingPatron.Email = updatedPatronDTO.Email ?? existingPatron.Email;

    // Save changes
    db.SaveChanges();

    return Results.Ok(existingPatron);
});

app.MapPut("/api/patrons/deactivate/{id}", (LacontesLibraryDbContext db, int id, IMapper mapper) =>
{
    // Retrieve the patron from the database
    var existingPatron = db.Patrons.Find(id);

    if (existingPatron == null)
    {
        return Results.NotFound($"Patron with ID {id} not found.");
    }
    // Deactivate the patron
    existingPatron.IsActive = false;
    // Save changes
    db.SaveChanges();
    // Map the updated patron to PatronDTO
    var ourPatronDTO = mapper.Map<PatronDTO>(existingPatron);
    return Results.Ok(ourPatronDTO);
});



app.MapPost("/api/checkouts", async (LacontesLibraryDbContext db, IMapper mapper, CreateCheckoutDTO createCheckoutDTO) =>
{
     // Check if the material exists // THESE HAVE TO BE ADDED EXPLICITLY OTHERWISE THE MAPPER WONT OWRK
     //NAV PROPERTIES ARENT OPTIONAL FOR OUR MAPPER WAY ITS SETUUP 
   var material = db.Materials
    .Include(m => m.Genre)
    .Include(m => m.MaterialType)
    .FirstOrDefault(m => m.Id == createCheckoutDTO.MaterialId);

if (material == null)
{
    return Results.NotFound($"Material with ID {createCheckoutDTO.MaterialId} not found.");
}

    // Check if the patron exists
    var patron = db.Patrons.SingleOrDefault(p => p.Id == createCheckoutDTO.PatronId);
    if (patron == null)
    {
        return Results.NotFound($"Patron with ID {createCheckoutDTO.PatronId} not found.");
    }

//Do not manually calculate the ID. The database will handle it.

var newCheckout = new Checkout
{
    MaterialId = createCheckoutDTO.MaterialId,
    PatronId = createCheckoutDTO.PatronId,
    CheckoutDate = DateTime.Today,
    ReturnDate = null, // or set appropriately,
    Material = material,
    Patron = patron
};

db.Checkouts.Add(newCheckout);


int checkoutID = newCheckout.Id; // Use the ID assigned by the database JUST FOR YOUR OWN INFO THIS ISNT ACTUALLY NEEDED

// What Happens During SaveChangesAsync
// When you add the newCheckout to db.Checkouts and call SaveChangesAsync, Entity Framework Core:

// Automatically associates the newCheckout with the appropriate Material and Patron based 
//on the MaterialId and PatronId foreign keys.
// Updates the Checkouts navigation properties in memory to reflect the changes.
// Why .Include is Not Needed
// The .Include method is used for loading related entities from the database. In this case:

// You are creating a new Checkout and explicitly setting the MaterialId and PatronId.
// EF Core uses these foreign keys to establish the relationships.
// There’s no need to load (.Include) the related Material or Patron 
//beforehand unless you need their data for some reason.
// WE DONT NEED ANY OF THAT BECUASE WE SPECIFIED THIS 
// modelBuilder.Entity<Material>()
//     .HasMany(m => m.Checkouts) // A Material can have many Checkouts
//     .WithOne(c => c.Material) // Each Checkout is for one Material
//     .HasForeignKey(c => c.MaterialId); // The foreign key is in the Checkout table

await db.SaveChangesAsync(); // The database will assign the ID automatically

var checkoutDTO = mapper.Map<CheckoutDTO>(newCheckout);

// var ourMaterial = db.Materials.FirstOrDefault(eachMaterial => eachMaterial.Id == createCheckoutDTO.MaterialId); 


var returnDTO = 
    new {
        CheckoutDTO = checkoutDTO,
        MaterialDTO = mapper.Map<MaterialDTO>(material)
        //mapper.Map<MaterialDTO>(db.Materials.FirstOrDefaultAsync(eachMaterial => eachMaterial.Id == createCheckoutDTO.MaterialId)) we adjusted this part it has to be the material we fetched w the navigation prop because mapper maps those nav prop
    };

return Results.Created($"/api/checkouts/{newCheckout}", returnDTO);

});

app.MapPut("/api/checkouts/{id}/return", (LacontesLibraryDbContext db, int id, IMapper mapper) =>
{
    // Find the checkout by ID
    var checkout = db.Checkouts.Find(id);

    if (checkout == null)
    {
        return Results.NotFound($"Checkout with ID {id} not found.");
    }

    // Update the ReturnDate to today's date
    checkout.ReturnDate = DateTime.Today;

    // Save changes to the database
    db.SaveChanges();

    // Map the updated checkout to CheckoutDTO
    var checkoutDTO = mapper.Map<CheckoutDTO>(checkout);

    // Return the updated checkout
    return Results.Ok(checkoutDTO);
});


app.MapGet("/api/materials/available", (LacontesLibraryDbContext db) =>
{
    return db.Materials
    .Where(m => m.OutOfCirculationSince == null)
    .Where(m => m.Checkouts.All(co => co.ReturnDate != null))
    .Select(material => new NSSMaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince
    })
    .ToList();
});



app.MapGet("/api/checkouts/overdue", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    return db.Checkouts
    .Include(p => p.Patron) // p co and m all rep the same variable btw just calling it diff things 
    .Include(co => co.Material) // Use 'co' here
    .ThenInclude(m => m.MaterialType) // Use 'm' here
    .Where(checkout => 
        (DateTime.Today - checkout.CheckoutDate).Days > 
        checkout.Material.MaterialType.CheckoutDays &&
        checkout.ReturnDate == null) // Use 'checkout' here
    .Select(ourCheckoutDTO => new NSSCheckoutDTO
    { Id = ourCheckoutDTO.Id, 
      MaterialId = ourCheckoutDTO.MaterialId,
      PatronId = ourCheckoutDTO.PatronId,
      CheckoutDate = ourCheckoutDTO.CheckoutDate,
      ReturnDate = ourCheckoutDTO.ReturnDate,
      PatronDTO = mapper.Map<PatronDTO>(ourCheckoutDTO.Patron),
      MaterialDTO = new NSSMaterialDTO {
        Id = ourCheckoutDTO.MaterialId,
        MaterialName = ourCheckoutDTO.Material.MaterialName,
        MaterialTypeId = ourCheckoutDTO.Material.MaterialTypeId,
        MaterialTypeDTO = new MaterialTypeDTO{
            Name = db.MaterialTypes.FirstOrDefault( mt => mt.Id == ourCheckoutDTO.Material.MaterialTypeId).Name,
            CheckoutDays = db.MaterialTypes.FirstOrDefault( mt => mt.Id == ourCheckoutDTO.Material.MaterialTypeId).CheckoutDays
        },
        GenreId = ourCheckoutDTO.Material.GenreId,
        OutOfCirculationSince = ourCheckoutDTO.Material.OutOfCirculationSince
      }
    }).ToList();
});

app.MapGet("/api/checkouts/overdue2", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    return db.Checkouts
        .Include(checkout => checkout.Patron) // Include Patron
        .Include(checkout => checkout.Material) // Include Material
        .ThenInclude(material => material.MaterialType) // Include MaterialType
        .Where(checkout =>
            (DateTime.Today - checkout.CheckoutDate).Days > 
            checkout.Material.MaterialType.CheckoutDays &&
            checkout.ReturnDate == null) // Filter overdue checkouts
        .Select(checkout => new CheckoutWithLateFeeDto
        {
            Id = checkout.Id,
            MaterialId = checkout.MaterialId,
            PatronId = checkout.PatronId,
            CheckoutDate = checkout.CheckoutDate,
            ReturnDate = checkout.ReturnDate,
            PatronDTO = mapper.Map<PatronDTO>(checkout.Patron),
            MaterialDTO = mapper.Map<NSSMaterialDTO>(checkout.Material)
        })
        .ToList();
});

//additional endpoints for assigment 11-21

app.MapGet("api/patrons/active", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var activePatrons = db.Patrons
        .Where(p => p.IsActive) // Only active patrons
        .Select(patron => mapper.Map<PatronDTO>(patron)) // Map to PatronDTO
        .ToList();

    return Results.Ok(activePatrons);
});

app.MapGet("api/patrons/lateFees", (LacontesLibraryDbContext db) =>
{
    try
    {
        var patrons = db.Patrons
    .Include(p => p.Checkouts) // Fetch related checkouts
        .ThenInclude(c => c.Material) // Include Material for each checkout
            .ThenInclude(m => m.MaterialType) // Include MaterialType for each material
    .ToList(); // Materialize data into memory

        // Process patrons and compute balances
        var patronsWithLateFees = patrons.Select(patron => new
        {
            FirstName = patron.FirstName,
            LastName = patron.LastName,
            Address = patron.Address,
            Email = patron.Email,
            IsActive = patron.IsActive,
            Balance = patron.Balance // Dynamically calculated
        }).ToList();

        return Results.Ok(patronsWithLateFees);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
        return Results.Problem("An error occurred while processing late fees.");
    }
});

app.MapGet("/api/checkouts/frontend", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var checkouts = db.Checkouts
        .Include(checkout => checkout.Material) // Include Material for each checkout
            .ThenInclude(material => material.MaterialType) // Include MaterialType
        .Include(checkout => checkout.Material.Genre) // Include Genre
        .Include(checkout => checkout.Patron) // Include Patron
        .ToList(); // Load data into memory to handle null checks manually

    var checkoutDTOs = checkouts.Select(checkout => new
    {
        CheckoutId = checkout.Id, // Include Checkout ID explicitly
        CheckoutDate = checkout.CheckoutDate,
        ReturnDate = checkout.ReturnDate,
        Material = checkout.Material != null ? new
        {
            MaterialId = checkout.Material.Id, // Include Material ID explicitly
            MaterialName = checkout.Material.MaterialName,
            MaterialType = checkout.Material.MaterialType != null ? new
            {
                Name = checkout.Material.MaterialType.Name,
                CheckoutDays = checkout.Material.MaterialType.CheckoutDays
            } : null, // Manual null check for MaterialType
            Genre = checkout.Material.Genre != null ? checkout.Material.Genre.Name : null // Manual null check for Genre
        } : null, // Manual null check for Material
        Patron = checkout.Patron != null ? new
        {
            PatronId = checkout.Patron.Id,
            FirstName = checkout.Patron.FirstName,
            LastName = checkout.Patron.LastName,
            Email = checkout.Patron.Email
        } : null // Manual null check for Patron
    }).ToList();

    return Results.Ok(checkoutDTOs);
});


app.MapGet("/api/materials/frontend", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var materials = db.Materials
        .Include(material => material.MaterialType) // Include MaterialType
        .Include(material => material.Genre) // Include Genre
        .Include(material => material.Checkouts) // Include Checkouts
            .ThenInclude(checkout => checkout.Patron) // Include Patron for each checkout
        .ToList(); // Load data into memory to handle null checks manually

    var materialDTOs = materials.Select(material => new
    {
        MaterialId = material.Id, // Include Material ID explicitly
        MaterialName = material.MaterialName, // Simplified Material Name
        MaterialType = material.MaterialType != null ? new
        {
            Name = material.MaterialType.Name,
            CheckoutDays = material.MaterialType.CheckoutDays
        } : null, // Manual null check for MaterialType
        Genre = material.Genre != null ? material.Genre.Name : null, // Manual null check for Genre
        Checkouts = material.Checkouts.Select(checkout => new
        {
            CheckoutId = checkout.Id, // Explicit Checkout ID
            CheckoutDate = checkout.CheckoutDate,
            ReturnDate = checkout.ReturnDate,
            Patron = checkout.Patron != null ? new
            {
                FirstName = checkout.Patron.FirstName,
                LastName = checkout.Patron.LastName,
                Email = checkout.Patron.Email
            } : null // Manual null check for Patron
        }).ToList()
    }).ToList();

    return Results.Ok(materialDTOs);
});


app.MapGet("/api/patrons/frontend", (LacontesLibraryDbContext db, IMapper mapper) =>
{
    var patrons = db.Patrons
        .Include(patron => patron.Checkouts) // Include Checkouts for Patron
            .ThenInclude(checkout => checkout.Material) // Include Material for each checkout
            .ThenInclude(material => material.MaterialType) // Include MaterialType
        .Include(patron => patron.Checkouts) // Include Checkouts for Patron
            .ThenInclude(checkout => checkout.Material.Genre) // Include Genre for Material
        .ToList(); // Load data into memory to handle null-propagation manually

    var patronDTOs = patrons.Select(patron => new
    {
        PatronId = patron.Id, // Include Patron ID explicitly
        PatronDTO = mapper.Map<PatronDTO>(patron), // Map Patron details
        Checkouts = patron.Checkouts.Select(checkout => new
        {
            CheckoutId = checkout.Id, // Include Checkout ID explicitly
            CheckoutDTO = new
            {
                checkout.CheckoutDate,
                checkout.ReturnDate
            },
            MaterialId = checkout.MaterialId, // Include Material ID explicitly
            MaterialName = checkout.Material?.MaterialName, // Check for null after data is loaded
            MaterialType = checkout.Material?.MaterialType != null ? checkout.Material.MaterialType.Name : null, // Manual null check
            Genre = checkout.Material?.Genre != null ? checkout.Material.Genre.Name : null // Manual null check
        }).ToList()
    }).ToList();

    return Results.Ok(patronDTOs);
});






app.Run(); 