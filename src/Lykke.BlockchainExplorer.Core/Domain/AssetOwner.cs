using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Domain
{
    public class AssetOwners
    {
        public Asset Asset { get; set; }
        public long BlockHeight { get; set; }  
        public IList<Owner> Owners { get; set; }
    }
}