using RapidPay.DAL.Models;


namespace RapidPay.Services.Interfaces
{
    public interface ICardService
    {
        Task<Card> GetCardAsync(int id);
        Task<Card> AddCardAsync(Card card);
        Task MakePaymentAsync(int id, decimal amount);
        Task<decimal> GetBalanceAsync(int id);
        Task<Card?> GetCardByIdAsync(int id);
    }
}