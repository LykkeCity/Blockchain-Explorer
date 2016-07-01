using Core.Bitcoin;
using Core.BitcoinNinja;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class IndexModel
    {
        public ILastBlockNinja LastBlock { get; set; }
    }
}