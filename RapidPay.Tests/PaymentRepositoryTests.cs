using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.DAL.Repositories;

namespace RapidPay.Tests.Repositories
{

    public class PaymentRepositoryTests : RepositoryTests
    {
        private readonly IPaymentRepository _repository;

        public PaymentRepositoryTests() : base()
        {
            _repository = new PaymentRepository(_context);
        }

        [Fact]
        public async Task AddPaymentAsync_ShouldReturnPaymentWhenCardExists()
        {
            //Arrange
            var payment = new Payment(1, 1000, DateTime.Now);

            //Act
            var result = await _repository.AddPaymentAsync(payment);

            //Assert
            Assert.Equal(payment, result);
            Assert.Contains(payment, _context.Payments);
        }

        [Fact]
        public async Task AddPaymentAsync_ShouldThrowErrorWhenCardNotExists()
        {
            //Arrange
            var payment = new Payment(6, 1000, DateTime.Now);

            //Act
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddPaymentAsync(payment));
        }

        [Fact]
        public async Task GetPaymentsByCardIdAsync_ShouldReturnPaymentsWhenCardExists()
        {
            //Arrange
            var card = _context.Cards.FirstOrDefault();

            //Act 
            var payments = await _repository.GetPaymentsByCardIdAsync(card.Id);

            //Assert
            Assert.NotEqual(0, payments.Count);
            Assert.Equal(card.Id, payments.First().CardId);
            Assert.Equal(card.Id, payments.Last().CardId);
        }

        [Fact]
        public async Task GetPaymentsByCardIdAsync_ShouldThrowExceptionWhenCardnotExists()
        {
            // Act
            var payments = await _repository.GetPaymentsByCardIdAsync(6);

            // Assert
            Assert.Empty(payments);
            Assert.Equal(0, payments.Count);
        }
    }
}
