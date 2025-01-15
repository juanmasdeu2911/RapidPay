using System.ComponentModel.DataAnnotations;

namespace RapidPay.DAL.Models
{
    /// <summary>
    /// Represents a transaction associated with a card.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets the unique identifier for the transaction.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the identifier of the associated card.
        /// </summary>
        [Required]
        public int CardId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the transaction.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the date of the transaction.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="cardId">The identifier of the associated card.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="date">The date of the transaction.</param>
        public Transaction(int cardId, decimal amount, DateTime date)
        {
            CardId = cardId;
            Amount = amount;
            Date = date;
        }
    }
}
