using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts.Services
{
    public interface IAssetService
    {
        IBlockchainDataProvider BlockchainDataProvider { get; set; }
        IAssetRepository AssetRepository { get; set; }

        Task<Asset> GetAsset(string id);
        Task<IList<Asset>> GetAssetList();
        Task<AssetOwners> GetAssetOwners(string id);
    }
}
