using System.Collections.Generic;
using Core.Bitcoin;
using Core.BitcoinNinja;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class BlockModel
    {
        public IBlockNinja Block { get; set ; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int Start { get; set; }
        public int Max { get; set; }
    }

   
}