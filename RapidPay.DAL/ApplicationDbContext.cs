using Microsoft.EntityFrameworkCore;
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
        /// Gets or sets the DbSet of transactions.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Card entity
            // The Card entity has a primary key, a card number, and a balance.
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number)
                      .IsRequired()
                      .HasMaxLength(15);
                entity.Property(e => e.Balance)
                      .IsRequired()
                      .HasPrecision(18, 2);
            });

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CardId)
                      .IsRequired();
                entity.Property(e => e.Amount)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(e => e.Date)
                      .IsRequired();
                entity.HasOne<Card>()
                      .WithMany()
                      .HasForeignKey(e => e.CardId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}