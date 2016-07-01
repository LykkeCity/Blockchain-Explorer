using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Domain
{
    public class Address
    {
        public string Hash { get; set; }
        public string ColoredAddress { get; set; }
        public string UncoloredAddress { get; set; }
        public long Balance { get; set; }
        public long TotalTransactions { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public IList<Asset> Assets { get; set; }
    }
}
