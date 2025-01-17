using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;


namespace RapidPay.DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Payment> _transactions;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
            _transactions = _context.Set<Payment>();
        }

        public async Task<Payment?> AddPaymentAsync(Payment payment)
        {
            using (var tran = _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _transactions.AddAsync(payment);
                    await _context.SaveChangesAsync();
                    await tran.Result.CommitAsync();
                    return payment;
                }
                catch
                {
                    await tran.Result.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IList<Payment>> GetPaymentsByCardIdAsync(int cardId)
        {
            return await _transactions.Where(x => x.CardId == cardId).ToListAsync();
        }

    }
}
