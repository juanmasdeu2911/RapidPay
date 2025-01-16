using RapidPay.DAL.Models;

namespace RapidPay.DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> AddTransactionAsync(Transaction transaction);
        Task<IList<Transaction>> GetTransactionsByCardId(int cardId);
    }
}
