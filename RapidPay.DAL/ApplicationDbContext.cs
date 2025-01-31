﻿using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Models;

namespace RapidPay.DAL.Data
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet of cards.
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of payments.
        /// </summary>
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Card entity
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number)
                      .IsRequired()
                      .HasMaxLength(15);
                entity.Property(e => e.Balance)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.HasMany(e => e.Payments)
                      .WithOne()
                      .HasForeignKey(p => p.CardId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Payment entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CardId)
                      .IsRequired();
                entity.Property(e => e.Amount)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(e => e.Date)
                      .IsRequired();
            });
        }
    }
}