using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinChainExplorerForAspNet5.Helpers
{
    public class NumberFormat
    {
        public string ToDecimal(decimal number)
        {
            return string.Format("{0:0.00######}", number);
        }
    }
}
