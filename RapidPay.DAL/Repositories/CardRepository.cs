using RapidPay.DAL.Models;
using RapidPay.DAL.Interfaces;

namespace RapidPay.DAL.Repositories
{
    /// <summary>
    /// Implements the repository for managing cards.
    /// </summary>
    public class CardRepository : ICardRepository
    {
        private readonly List<Card> _cards = new();

        /// <summary>
        /// Creates a new card asynchronously.
        /// </summary>
        /// <param name="card">The card to create.</param>
        /// <returns>The created card.</returns>
        public async Task<Card> CreateCardAsync(Card card)
        {
            _cards.Add(card);
            return await Task.FromResult(card);
        }

        /// <summary>
        /// Retrieves a card by its number asynchronously.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <returns>The card with the specified number.</returns>
        public async Task<Card?> GetCardAsync(string cardNumber)
        {
            var card = _cards.FirstOrDefault(c => c.Number == cardNumber);
            return await Task.FromResult(card);
        }

        /// <summary>
        /// Updates an existing card asynchronously.
        /// </summary>
        /// <param name="card">The card to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> UpdateCardAsync(Card card)
        {
            var existingCard = _cards.FirstOrDefault(c => c.Id == card.Id);
            if (existingCard == null)
            {
                return await Task.FromResult(false);
            }

            existingCard.Number = card.Number;
            existingCard.Balance = card.Balance;
            return await Task.FromResult(true);
        }
    }
}
