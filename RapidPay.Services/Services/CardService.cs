using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;
using RapidPay.Services.Interfaces;

namespace RapidPay.Services.Services
{
    public class CardService : ICardService
    {
        public ICardRepository _repository { get; set; }

        public CardService(ICardRepository repository) {
            _repository = repository;
        }
               
        public async Task<Card?> GetCardByIdAsync(int id)
        {
            return await _repository.GetCardByIdAsync(id);
        }

        public async Task<Card> AddCardAsync(Card card)
        {
            return await _repository.CreateCardAsync(card);
        }

        public Task MakePaymentAsync(int id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetBalanceAsync(int id)
        {
            Card? card = await _repository.GetCardByIdAsync(id);

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
