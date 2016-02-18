using System;
using System.Threading.Tasks;

namespace Core.Bitcoin
{

    public interface IBitcoinBlock
    {
        string Hash { get; }
        DateTime Time { get; }
        long Height { get; }
        string Previousblockhash { get; }
        string Nextblockhash { get; }
        double Difficulty { get; }
        string Merkleroot { get; }
        long Nonce { get; }
        long TotalTransactions { get; }
    }


    public class BitcoinBlock: IBitcoinBlock
    {
        public string Hash { get; set; }
        public DateTime Time { get; set; }
        public long Height { get; set; }
        public string Previousblockhash { get; set; }
        public string Nextblockhash { get; set; }
        public double Difficulty { get; set; }
        public string Merkleroot { get; set; }
        public long Nonce { get; set; }
        public long TotalTransactions { get; set; }

    }

    public interface IBitcoinBlockRepository
    {
        Task SaveAsync(IBitcoinBlock bitcoinBlock);
        Task<IBitcoinBlock> GetAsync(string block);
    }
}