using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.Services.Fees;
using RapidPay.Services.Interfaces;

namespace RapidPay.Services.Services
{
    public class CardService : ICardService
    {
        public ICardRepository _cardRepository { get; set; }
        public IPaymentRepository _paymentRepository { get; set; }

        public CardService(ICardRepository cardRepository, IPaymentRepository transactionRepository)
        {
            _cardRepository = cardRepository;
            _paymentRepository = transactionRepository;
        }

        public async Task<Card?> GetCardByIdAsync(int id)
        {
            return await _cardRepository.GetCardByIdAsync(id);
        }

        public async Task<Card> AddCardAsync(Card card)
        {
            return await _cardRepository.CreateCardAsync(card);
        }

        public async Task<Card?> MakePaymentAsync(int id, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));
            }

            var card = await _cardRepository.GetCardByIdAsync(id);
            if (card == null)
            {
                throw new KeyNotFoundException($"Card id {id} not found");
            }

            var fee = UniversalFeesExchange.Instance.GetCurrentFee();
            var totalAmount = amount * fee;

            if (card.Balance < totalAmount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            card.Balance -= totalAmount;
            await _cardRepository.UpdateCardAsync(card);

            var transaction = new Payment(card.Id, totalAmount, DateTime.UtcNow);
            await _paymentRepository.AddPaymentAsync(transaction);

            return card;
        }

        public async Task<decimal> GetBalanceAsync(int id)
        {
            Card? card = await _cardRepository.GetCardByIdAsync(id);

            if (card == null)
                throw new ApplicationException($"Card id {id} not found");

            return card.Balance;
        }

        public async Task<Card> GetCardAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Payment>> GetPaymentListAsync(int id)
        {
            return await _paymentRepository.GetPaymentsByCardIdAsync(id);
        }
    }
}
