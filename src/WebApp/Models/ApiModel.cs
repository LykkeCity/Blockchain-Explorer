using System;
using Core.Bitcoin;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class ApiTxModel : ITransaction
    {
        public string Txid { get; set; }
        public string Blockhash { get; set; }
        public long Confirmations { get; set; }
        public DateTime Time { get; set; }
        public long Height { get; set; }
    }
}