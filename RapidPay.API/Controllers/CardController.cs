
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.DAL.Models;
using RapidPay.Services.Interfaces;

namespace RapidPay.API.Controllers
{
    /// <summary>
    /// Controller for managing card operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardController"/> class.
        /// </summary>
        /// <param name="cardService">The card service.</param>
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Gets the details of a card by its ID.
        /// </summary>
        /// <param name="id">ID of the card.</param>
        /// <returns>Details of the card.</returns>
        /// <response code="200">Returns the card details</response>
        /// <response code="404">If the card is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Card), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCard(int id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            if (card == null)
            {
                return NotFound(new { Message = "Card not found" });
            }
            return Ok(card);
        }

        /// <summary>
        /// Creates a new card.
        /// </summary>
        /// <param name="card">Details of the card to create.</param>
        /// <returns>The created card.</returns>
        /// <response code="201">Returns the newly created card</response>
        /// <response code="400">If the card details are invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(Card), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is invalid
            }

            var result = await _cardService.AddCardAsync(card);
            return CreatedAtAction(nameof(GetCard), new { id = result.Id }, result);
        }

        /// <summary>
        /// Makes a payment from a card.
        /// </summary>
        /// <param name="id">ID of the card.</param>
        /// <param name="amount">Amount of the payment.</param>
        /// <returns>Result of the operation.</returns>
        /// <response code="200">If the payment is successful</response>
        /// <response code="400">If an error occurs during the payment</response>
        [HttpPost("{id}/pay")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Pay(int id, [FromBody] decimal amount)
        {
            try
            {
                await _cardService.MakePaymentAsync(id, amount);
                return Ok(new { Message = "Fees successful" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // Returns 400 Bad Request if an exception occurs
            }
        }

        /// <summary>
        /// Gets the balance of a card by its ID.
        /// </summary>
        /// <param name="id">ID of the card.</param>
        /// <returns>Current balance of the card.</returns>
        /// <response code="200">Returns the current balance of the card</response>
        /// <response code="400">If an error occurs while retrieving the balance</response>
        [HttpGet("{id}/balance")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBalance(int id)
        {
            try
            {
                var balance = await _cardService.GetBalanceAsync(id);
                return Ok(new { Balance = balance });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // Returns 400 Bad Request if an exception occurs
            }
        }
    }
}
