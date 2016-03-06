using System.Collections.Generic;
using Core.Bitcoin;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class BlockModel
    {
        public IBitcoinBlock Block { get; set ; }
        public IEnumerable<ITransaction> Transactions  { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        // public IDictionary<string, TransactionModel> Transactions { get; set; }
    }

   
}