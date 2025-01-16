using RapidPay.DAL.Models;

namespace RapidPay.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> AddPaymentAsync(Payment transaction);
        Task<IList<Payment>> GetPaymentsByCardId(int cardId);
    }
}
