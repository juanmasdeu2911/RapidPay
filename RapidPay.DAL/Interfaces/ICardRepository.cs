using RapidPay.DAL.Models;

namespace RapidPay.DAL.Interfaces
{

    /// <summary>
    /// Defines the repository interface for managing cards.
    /// </summary>
    public interface ICardRepository
    {
        /// <summary>
        /// Creates a new card asynchronously.
        /// </summary>
        /// <param name="card">The card to create.</param>
        /// <returns>The created card.</returns>
        Task<Card> CreateCardAsync(Card card);

        /// <summary>
        /// Retrieves a card by its number asynchronously.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <returns>The card with the specified number.</returns>
        Task<Card?> GetCardAsync(string cardNumber);

        /// <summary>
        /// Updates an existing card asynchronously.
        /// </summary>
        /// <param name="card">The card to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> UpdateCardAsync(Card card);
    }
}
