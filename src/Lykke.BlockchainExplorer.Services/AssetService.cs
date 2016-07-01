using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Services
{
    public class AssetService : IAssetService
    {
        public IBlockchainDataProvider BlockchainDataProvider { get; set; }
        public IAssetRepository AssetRepository { get; set; }
        public ILog Log { get; set; }

        public AssetService(IBlockchainDataProvider blockchainProvider,
                            IAssetRepository assetRepository,
                            ILog log)
        {
            BlockchainDataProvider = blockchainProvider; 
            AssetRepository = assetRepository;
            Log = log;
        }

        public async Task<Asset> GetAsset(string id)
        {
            Asset asset;

            try
            {
                asset = await AssetRepository.GetById(id); 
                 
                if(asset == null)
                {
                    asset = await BlockchainDataProvider.GetAsset(id);
                    await AssetRepository.Save(asset);
                } 
            }
            catch(Exception ex) 
            {
                await Log.WriteError(this.GetType().ToString(), "GetAsset", $"asset_id:{id}", ex, DateTime.Now);
                return null;
            }

            return asset;
        }
          
        public async Task<IList<Asset>> GetAssetList()
        {
            IList<Asset> assets; 

            try
            {
                assets = await AssetRepository.GetAssets();
            } 
            catch(Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetAssetList", "asset_list", ex, DateTime.Now);
                return null;
            }

            return assets;
        }

        public async Task<AssetOwners> GetAssetOwners(string id)
        {
            AssetOwners assetOwner;

            try
            {
                var lastBlock = await BlockchainDataProvider.GetLastBlock();

                assetOwner = await AssetRepository.GetAssetOwners(id);
                assetOwner.BlockHeight = lastBlock.Height;
            }
            catch(Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetAssetOwners", $"asset_id{id}", ex, DateTime.Now);
                return null;
            }

            return assetOwner;
        }
    }
}
