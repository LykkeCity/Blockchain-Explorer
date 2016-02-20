using System.Collections.Generic;
using Core.Bitcoin;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class BlockModel
    {
        public IBitcoinBlock Block { get; set ; }
        public IDictionary<string, TransactionModel> Transactions { get; set; }
    }

   
}