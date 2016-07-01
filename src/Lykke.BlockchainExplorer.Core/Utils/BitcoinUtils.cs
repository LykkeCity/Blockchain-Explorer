using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Utils
{
    public static class BitcoinUtils
    {
        public static decimal SatoshiToBtc(long satoshi)
        {
            return (decimal)(satoshi * 0.00000001);
        }
    }
}