using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.DAL.Repositories;

namespace RapidPay.Tests.Services
{
    public class ServiceTests : IDisposable
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ICardRepository _cardRepository;
        protected readonly IPaymentRepository _paymentRepository;

        public ServiceTests()
        {
            var databaseName = GenerateDatabseRandomName();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseSqlServer($"Server=.\\rapidpayexpress;Database={databaseName};User ID=sa;Password=admin1234;Encrypt=True;TrustServerCertificate=True")
            .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            SeedDatabase();

            _cardRepository = new CardRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
        }

        private void SeedDatabase()
        {
            _context.Cards.Add(new Card("3456789012345", 25000));
            _context.Cards.Add(new Card("32109876543210", 50000));

            _context.Payments.Add(new Payment(1, 5000, DateTime.Now));
            _context.Payments.Add(new Payment(2, 10000, DateTime.Now));
            _context.Payments.Add(new Payment(1, 5000, DateTime.Now));
            _context.Payments.Add(new Payment(1, 5000, DateTime.Now));
            _context.Payments.Add(new Payment(2, 10000, DateTime.Now));
            _context.Payments.Add(new Payment(2, 10000, DateTime.Now));

            _context.SaveChanges();

        }

        private string GenerateDatabseRandomName()
        {
            Random random = new Random();
            int randomNumber = random.Next(100, 1000); // Generates a random number between 100 and 999
            return $"RapidPayTest_{randomNumber}";
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
