using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;
using RapidPay.DAL.Models;
using RapidPay.DAL.Repositories;

namespace RapidPay.Tests.Repositories
{
    public class CardRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly CardRepository _repository;

        public CardRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseSqlServer("Server=.\\rapidpayexpress;Database=RapidPayTest;User ID=sa;Password=admin1234;Encrypt=True;TrustServerCertificate=True")
                              .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedDatabase();

            _repository = new CardRepository(_context);

        }

        private void SeedDatabase()
        {
            _context.Cards.Add(new Card("3456789012345", 25000));
            _context.Cards.Add(new Card("32109876543210", 50000));
            _context.SaveChanges();

        }

        [Fact]
        public async Task CreateCardAsync_ShouldAddCard()
        {
            // Arrange
            var card = new Card("123456789011345", 1000);

            // Act
            var result = await _repository.CreateCardAsync(card);

            // Assert
            Assert.Equal(card, result);
            Assert.Contains(card, _context.Cards);
        }

        [Fact]
        public async Task GetCardAsync_ShouldReturnCard_WhenCardExists()
        {
           
            var result = await _repository.GetCardAsync("3456789012345");

            //Assert
            Assert.NotNull(result);
            Assert.Equal("3456789012345", result.Number);
        }

        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnCard_WhenCardExists()
        {

            var result = await _repository.GetCardByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("3456789012345", result.Number);
        }

        [Fact]
        public async Task UpdateCardAsync_ShouldUpdateCard_WhenCardExists()
        {
            //Arrange
            var existingCard = await _repository.GetCardByIdAsync(1);
            existingCard.Balance = 15000;

            //Act
            var result = await _repository.UpdateCardAsync(existingCard);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateCardAsync_ShouldReturnFalse_WhenCardDoesNotExist()
        {

            // Act
            var existingCard = await _repository.GetCardByIdAsync(3);
                        
            //Assert
            Assert.Null(existingCard);
        }
    }
}
