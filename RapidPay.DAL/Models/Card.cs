using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.DAL.Models
{
    /// <summary>
    /// Represents a payment card with a unique identifier, number, and balance.
    /// </summary>
    [PrimaryKey(nameof(Id))]
    public class Card
    {
        /// <summary>
        /// Gets the unique identifier for the card.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the card number in a 15-digit format.
        /// </summary>
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Card number must be 15 digits.")]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the balance of the card.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative value.")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="number">The card number.</param>
        /// <param name="balance">The initial balance of the card.</param>
        public Card(string number, decimal balance)
        {
            Number = number;
            Balance = balance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        public Card() { }
    }
}
