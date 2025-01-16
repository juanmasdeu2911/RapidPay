using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.Services.Interfaces;
using RapidPay.Services.Payment;

namespace RapidPay.Services.Services
{
    public class CardService : ICardService
    {
        public ICardRepository _cardRepository { get; set; }
        public ITransactionRepository _transactionRepository { get; set; }

        public CardService(ICardRepository cardRepository, ITransactionRepository transactionRepository)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
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
                throw new ApplicationException("Amount greater than zero");
            }

            var card = await _cardRepository.GetCardByIdAsync(id);
            if (card == null)
            {
                throw new ApplicationException($"Card id {id} not found");
            }

            var fee = UniversalFeesExchange.Instance.GetCurrentFee();
            var totalAmount = amount * fee;

            if (card.Balance < totalAmount)
            {
                throw new ApplicationException("Insufficient funds");
            }

            card.Balance -= totalAmount;
            await _cardRepository.UpdateCardAsync(card);

            var transaction = new Transaction(card.Id, totalAmount, DateTime.UtcNow);
            await _transactionRepository.AddTransactionAsync(transaction);
            return card;
        }

        public async Task<decimal> GetBalanceAsync(int id)
        {
            Card? card = await _cardRepository.GetCardByIdAsync(id);

            if (card == null)
                throw new ApplicationException($"Card id {id} not found");

            return card.Balance;
        }

        public Task<Card> GetCardAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
