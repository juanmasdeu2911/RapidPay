using Microsoft.EntityFrameworkCore;
using Moq;
using RapidPay.DAL.Data;
using RapidPay.DAL.Models;
using RapidPay.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RapidPay.Tests.Repositories
{
    public class CardRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<DbSet<Card>> _mockDbSet;
        private readonly CardRepository _repository;

        public CardRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase(databaseName: "TestDatabase")
                              .Options;
            _mockContext = new Mock<ApplicationDbContext>(options);
            _mockDbSet = new Mock<DbSet<Card>>();
            _mockContext.Setup(m => m.Set<Card>()).Returns(_mockDbSet.Object);

            _repository = new CardRepository(_mockContext.Object);
        }

        [Fact]
        public async Task CreateCardAsync_ShouldAddCard()
        {
            // Arrange
            var card = new Card("123456789012345", 1000);

            // Act
            await _repository.CreateCardAsync(card);

            // Assert
            _mockDbSet.Verify(m => m.AddAsync(card, default), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetCardAsync_ShouldReturnCard_WhenCardExists()
        {           
                // Arrange
                var cardId = 1;
                var card = new Card("123456789012345", 1000);
                typeof(Card).GetProperty("Id")?.SetValue(card, cardId);
                var data = new List<Card> { card }.AsQueryable();

                var mockAsyncEnumerable = new Mock<IAsyncEnumerable<Card>>();
                var mockAsyncEnumerator = new Mock<IAsyncEnumerator<Card>>();

                mockAsyncEnumerator.Setup(m => m.Current).Returns(data.First());
                mockAsyncEnumerator.SetupSequence(m => m.MoveNextAsync())
                                   .ReturnsAsync(true)
                                   .ReturnsAsync(false);
                mockAsyncEnumerator.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask);

                mockAsyncEnumerable.Setup(m => m.GetAsyncEnumerator(default)).Returns(mockAsyncEnumerator.Object);

                _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(data.Provider);
                _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(data.Expression);
                _mockDbSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(data.ElementType);
                _mockDbSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
                _mockDbSet.As<IAsyncEnumerable<Card>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(mockAsyncEnumerator.Object);

                // Act
                var result = await _repository.GetCardByIdAsync(cardId);

                // Assert
                Assert.Equal(card, result);      


        }

        [Fact]
        public async Task GetCardByIdAsync_ShouldReturnCard_WhenCardExists()
        {
            // Arrange
            var cardId = 1;
            var card = new Card("123456789012345", 1000);
            typeof(Card).GetProperty("Id").SetValue(card, cardId);
            var data = new List<Card> { card }.AsQueryable();
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Act
            var result = await _repository.GetCardByIdAsync(cardId);

            // Assert
            Assert.Equal(card, result);
        }

        [Fact]
        public async Task UpdateCardAsync_ShouldUpdateCard_WhenCardExists()
        {
            // Arrange
            var cardId = 1;
            var card = new Card("123456789012345", 1000);
            typeof(Card).GetProperty("Id").SetValue(card, cardId);
            var data = new List<Card> { card }.AsQueryable();
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var updatedCard = new Card("543210987654321", 500);
            typeof(Card).GetProperty("Id").SetValue(updatedCard, cardId);

            // Act
            var result = await _repository.UpdateCardAsync(updatedCard);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedCard.Number, card.Number);
            Assert.Equal(updatedCard.Balance, card.Balance);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateCardAsync_ShouldReturnFalse_WhenCardDoesNotExist()
        {
            // Arrange
            var cardId = 1;
            var updatedCard = new Card("543210987654321", 500);
            typeof(Card).GetProperty("Id").SetValue(updatedCard, cardId);

            // Act
            var result = await _repository.UpdateCardAsync(updatedCard);

            // Assert
            Assert.False(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }
}
