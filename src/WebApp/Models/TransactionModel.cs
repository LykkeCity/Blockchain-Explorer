using System;
using System.Collections.Generic;
using System.Linq;
using Core.Bitcoin;
using Core.BitcoinNinja;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class TransactionViewModel
    {
        public ITransactionNinja Transaction { get; set; }
    }

    public class TransactionModel
    {
        public IEnumerable<IOutput> Outputs { get; set; }
        public IEnumerable<IInput> Inputs { get; set; }
        public ITransaction Transaction { get; set; }
        public DateTime Time { get; set; }
        public long Confirmations { get; set; }

        public double Fee
        {
            get
            {
                var inputsSum = Inputs.Sum(itm => itm.Value);
                if (Math.Abs(inputsSum) != 0)
                    return inputsSum - Outputs.Sum(itm => itm.Value);
                return 0;
            }
        }
    }
}