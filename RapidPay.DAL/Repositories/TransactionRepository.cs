using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Models;


namespace RapidPay.DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Transaction> _transactions;

        public TransactionRepository(ApplicationDbContext context) 
        {
            _context = context;
            _transactions = _context.Set<Transaction>();
        }

        public async Task<Transaction?> AddTransactionAsync(Transaction transaction)
        {
            await _transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
       
        public async Task<IList<Transaction>> GetTransactionsByCardId(int cardId)
        {
            return await _transactions.Where(x => x.CardId == cardId).ToListAsync();
        }
      
    }
}
