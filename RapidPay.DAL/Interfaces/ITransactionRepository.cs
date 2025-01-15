using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPay.DAL.Models;

namespace RapidPay.DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> AddTransactionAsync(Transaction transaction);
        Task<IList<Transaction>> GetTransactionsByCardId(int cardId);
    }
}
