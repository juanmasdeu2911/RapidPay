using Moq;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.Services.Interfaces;
using RapidPay.Services.Services;

namespace RapidPay.Tests.Services
{
    public class CardServiceTests
    {
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IUniversalFeesExchangeService> _universalFeesExchangeMock;
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardRepositoryMock = new Mock<ICardRepository>();
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _universalFeesExchangeMock = new Mock<IUniversalFeesExchangeService>();

            _cardService = new CardService(
                _cardRepositoryMock.Object,
                _paymentRepositoryMock.Object,
                _universalFeesExchangeMock.Object
            );
        }

        [Fact]
        public async Task MakePaymentAsync_ShouldThrowArgumentException_WhenAmountIsZeroOrLess()
        {
            // Arrange
            var amount = 0m;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _cardService.MakePaymentAsync(1, amount));
        }

        [Fact]
        public async Task MakePaymentAsync_ShouldThrowKeyNotFoundException_WhenCardNotFound()
        {
            // Arrange
            _cardRepositoryMock.Setup(repo => repo.GetCardByIdAsync(It.IsAny<int>())).ReturnsAsync((Card?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.MakePaymentAsync(1, 100m));
        }

        [Fact]
        public async Task MakePaymentAsync_ShouldThrowInvalidOperationException_WhenInsufficientFunds()
        {
            // Arrange
            var card = new Card("123456789012345", 50m);
            _cardRepositoryMock.Setup(repo => repo.GetCardByIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            _universalFeesExchangeMock.Setup(service => service.GetCurrentFee()).Returns(2m);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _cardService.MakePaymentAsync(1, 100m));
        }

        [Fact]
        public async Task MakePaymentAsync_ShouldUpdateCardBalance_WhenPaymentIsSuccessful()
        {
            // Arrange
            var card = new Card("123456789012345", 200m);
            _cardRepositoryMock.Setup(repo => repo.GetCardByIdAsync(It.IsAny<int>())).ReturnsAsync(card);
            _universalFeesExchangeMock.Setup(service => service.GetCurrentFee()).Returns(1.5m);
            _cardRepositoryMock.Setup(repo => repo.UpdateCardAsync(It.IsAny<Card>())).ReturnsAsync(true);
            _paymentRepositoryMock.Setup(repo => repo.AddPaymentAsync(It.IsAny<Payment>())).ReturnsAsync(new Payment(1, 150m, DateTime.UtcNow));

            // Act
            var result = await _cardService.MakePaymentAsync(1, 100m);

            // Assert
            Assert.Equal(50m, result.Balance);
            _cardRepositoryMock.Verify(repo => repo.UpdateCardAsync(It.Is<Card>(c => c.Balance == 50m)), Times.Once);
            _paymentRepositoryMock.Verify(repo => repo.AddPaymentAsync(It.IsAny<Payment>()), Times.Once);
        }
    }
}
