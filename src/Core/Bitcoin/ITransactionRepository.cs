using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface ITransaction
    {
        string Txid { get; }
        string Blockhash { get; }
        long Confirmations { get; }
        DateTime Time { get; }
        long Height { get; }
    }

    public class Transaction : ITransaction
    {
        public string Txid { get; set; }
        public string Blockhash { get; set; }
        public long Confirmations { get; set; }
        public DateTime Time { get; set; }
        public long Height { get; set; }
    }

    public interface ITransactionRepository
    {
        Task SaveAsync(Transaction transaction);
        Task<IEnumerable<ITransaction>> GetAsync(string hashBlock);
        Task<ITransaction> GetTransaction(string tx);
    }

}