using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lykke.BlockchainExplorer.UI.Models
{
    public class AssetOwnersModel
    {
        public Asset Asset { get; set; }
        public AssetOwners AssetOwners { get; set; }
        public long Total { get; set; }
    }
}