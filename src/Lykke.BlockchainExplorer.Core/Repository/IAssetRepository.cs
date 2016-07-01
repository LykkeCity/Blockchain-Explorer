using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Repository
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<bool> Exists(string id);
        Task<IList<Asset>> GetAssets();
        Task<AssetOwners> GetAssetOwners(string id);
    }
}