using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Domain
{
    public class Block
    {
        public string Hash { get; set; }
        public long Height { get; set; }
        public DateTime Time { get; set; }
        public long Confirmations { get; set; }
        public double Difficulty { get; set; }
        public string MerkleRoot { get; set; }
        public long Nonce { get; set; }
        public int TotalTransactions { get; set; }
        public string PreviousBlock { get; set; }
        public IList<Transaction> Transactions { get; set; }
    }
}
