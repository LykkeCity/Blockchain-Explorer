using System.Collections.Generic;
using Core.Bitcoin;

namespace BitcoinChainExplorerForAspNet5.Models
{
    public class AssetsViewModel 
    {
        public IAssets Asset { get; set; }
        
    }

    public class AssetsOwnersViewModel
    {
        public IAssetsOwners AssetsOwners { get; set; }
        public long Total { get; set; }
        public IAssets Asset { get; set; }
    }
}