﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LacontesLibrary.Migrations
{
    [DbContext(typeof(LacontesLibraryDbContext))]
    [Migration("20241119194005_AddNewFeatureOrFix")]
    partial class AddNewFeatureOrFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Models.Checkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckoutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaterialId")
                        .HasColumnType("integer");

                    b.Property<int>("PatronId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("PatronId");

                    b.ToTable("Checkouts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CheckoutDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 13,
                            PatronId = 1
                        },
                        new
                        {
                            Id = 2,
                            CheckoutDate = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 2,
                            PatronId = 2,
                            ReturnDate = new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CheckoutDate = new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 15,
                            PatronId = 3
                        },
                        new
                        {
                            Id = 4,
                            CheckoutDate = new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 17,
                            PatronId = 4,
                            ReturnDate = new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            CheckoutDate = new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 5,
                            PatronId = 1
                        },
                        new
                        {
                            Id = 6,
                            CheckoutDate = new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaterialId = 16,
                            PatronId = 2
                        });
                });

            modelBuilder.Entity("Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Science Fiction"
                        },
                        new
                        {
                            Id = 2,
                            Name = "History"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Biography"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Mystery"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Fantasy"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Romance"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Horror"
                        });
                });

            modelBuilder.Entity("Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaterialTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("OutOfCirculationSince")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MaterialTypeId");

                    b.ToTable("Materials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GenreId = 1,
                            MaterialName = "Dune",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            GenreId = 1,
                            MaterialName = "1984",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 3,
                            GenreId = 2,
                            MaterialName = "A Brief History of Time",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 4,
                            GenreId = 3,
                            MaterialName = "Becoming",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 5,
                            GenreId = 4,
                            MaterialName = "Sherlock Holmes",
                            MaterialTypeId = 4
                        },
                        new
                        {
                            Id = 6,
                            GenreId = 5,
                            MaterialName = "Harry Potter",
                            MaterialTypeId = 4
                        },
                        new
                        {
                            Id = 7,
                            GenreId = 6,
                            MaterialName = "Pride and Prejudice",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 8,
                            GenreId = 7,
                            MaterialName = "The Shining",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 9,
                            GenreId = 5,
                            MaterialName = "The Hobbit",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 10,
                            GenreId = 5,
                            MaterialName = "The Witcher",
                            MaterialTypeId = 4
                        },
                        new
                        {
                            Id = 11,
                            GenreId = 6,
                            MaterialName = "The Great Gatsby",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 12,
                            GenreId = 7,
                            MaterialName = "It",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 13,
                            GenreId = 1,
                            MaterialName = "The Martian",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 14,
                            GenreId = 2,
                            MaterialName = "Cosmos",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 15,
                            GenreId = 6,
                            MaterialName = "The Catcher in the Rye",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 16,
                            GenreId = 1,
                            MaterialName = "The Road",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 17,
                            GenreId = 2,
                            MaterialName = "Sapiens",
                            MaterialTypeId = 1
                        });
                });

            modelBuilder.Entity("Models.MaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckoutDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MaterialTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CheckoutDays = 21,
                            Name = "Book"
                        },
                        new
                        {
                            Id = 2,
                            CheckoutDays = 7,
                            Name = "Periodical"
                        },
                        new
                        {
                            Id = 3,
                            CheckoutDays = 14,
                            Name = "CD"
                        },
                        new
                        {
                            Id = 4,
                            CheckoutDays = 14,
                            Name = "DVD"
                        });
                });

            modelBuilder.Entity("Models.Patron", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patrons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            IsActive = true,
                            LastName = "Doe"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Elm St",
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            IsActive = true,
                            LastName = "Smith"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Oak St",
                            Email = "emily.johnson@example.com",
                            FirstName = "Emily",
                            IsActive = true,
                            LastName = "Johnson"
                        },
                        new
                        {
                            Id = 4,
                            Address = "101 Pine St",
                            Email = "michael.brown@example.com",
                            FirstName = "Michael",
                            IsActive = true,
                            LastName = "Brown"
                        },
                        new
                        {
                            Id = 5,
                            Address = "321 Cedar St",
                            Email = "sarah.davis@example.com",
                            FirstName = "Sarah",
                            IsActive = true,
                            LastName = "Davis"
                        },
                        new
                        {
                            Id = 6,
                            Address = "654 Maple St",
                            Email = "david.wilson@example.com",
                            FirstName = "David",
                            IsActive = false,
                            LastName = "Wilson"
                        });
                });

            modelBuilder.Entity("Models.Checkout", b =>
                {
                    b.HasOne("Models.Material", "Material")
                        .WithMany("Checkouts")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Patron", "Patron")
                        .WithMany("Checkouts")
                        .HasForeignKey("PatronId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Patron");
                });

            modelBuilder.Entity("Models.Material", b =>
                {
                    b.HasOne("Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.MaterialType", "MaterialType")
                        .WithMany()
                        .HasForeignKey("MaterialTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("MaterialType");
                });

            modelBuilder.Entity("Models.Material", b =>
                {
                    b.Navigation("Checkouts");
                });

            modelBuilder.Entity("Models.Patron", b =>
                {
                    b.Navigation("Checkouts");
                });
#pragma warning restore 612, 618
        }
    }
}
