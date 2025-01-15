using RapidPay.DAL.Models;
using RapidPay.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;

namespace RapidPay.DAL.Repositories
{
    /// <summary>
    /// Implements the repository for managing cards.
    /// </summary>
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Card> _cards;

        public CardRepository(ApplicationDbContext context)
        {
            _context = context;
            _cards = _context.Set<Card>();
        }

        /// <summary>
        /// Creates a new card asynchronously.
        /// </summary>
        /// <param name="card">The card to create.</param>
        /// <returns>The created card.</returns>
        public async Task<Card> CreateCardAsync(Card card)
        {
            await _cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
        }

        /// <summary>
        /// Retrieves a card by its number asynchronously.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <returns>The card with the specified number.</returns>
        public async Task<Card?> GetCardAsync(string cardNumber)
        {
            return await _cards.FirstOrDefaultAsync(c => c.Number == cardNumber);
        }

        /// <summary>
        /// Retrieves a card by its identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Card?> GetCardByIdAsync(int id)
        {
            return await _cards.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Updates an existing card asynchronously.
        /// </summary>
        /// <param name="card">The card to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> UpdateCardAsync(Card card)
        {
            var existingCard = await _cards.FirstOrDefaultAsync(c => c.Id == card.Id);
            if (existingCard == null)
            {
                return false;
            } 

            existingCard.Number = card.Number;
            existingCard.Balance = card.Balance;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
