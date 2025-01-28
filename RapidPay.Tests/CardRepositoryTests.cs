using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.DAL.Repositories;

namespace RapidPay.Tests.Repositories
{
    public class CardRepositoryTests : RepositoryTests
    {
        private readonly ICardRepository _repository;

        public CardRepositoryTests() : base()
        {
            _repository = new CardRepository(_context);
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
