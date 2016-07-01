using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Domain
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public long Confirmations { get; set; }
        public DateTime Time { get; set; }
        public string Blockhash { get; set; }
        public IList<In> TransactionIn { get; set; }
        public IList<Out> TransactionsOut { get; set; }
        public bool IsColor { get; set; }
        public bool IsCoinBase { get; set; }
        public string Hex { get; set; }
        public long Fees { get; set; }
        public Block Block { get; set; }
        public IList<Asset> Assets { get; set; }

        public long TotalOut
        {
            get
            {
                if (TransactionsOut == null) return 0;

                return TransactionsOut.Sum(x => x.Value);
            }
        }
    }

    public class In
    {
        public string TransactionId { get; set; }
        public string Address { get; set; }
        public int Index { get; set; }
        public long Value { get; set; }
        public string AssetId { get; set; }
        public long Quantity { get; set; }
    }

    public class Out
    {
        public string TransactionId { get; set; }
        public long Value { get; set; }
        public string Address { get; set; }
        public int Index { get; set; }
        public string AssetId { get; set; }
        public long Quantity { get; set; }
    }
}