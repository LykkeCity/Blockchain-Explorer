using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.BlockchainDataProvider.Coinprism.Client;
using Lykke.BlockchainExplorer.Settings;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.Coinprism
{
    public class AssetAdapter : IAssetProvider
    {
        private CoinprismClient _client;

        public AssetAdapter()
        { 
            var settings = new CoinprismApiSettings()
            {
                Network = AppSettings.Network,
                UrlMain = AppSettings.CoinprismUrlMain,
                UrlTest = AppSettings.CoinprismUrlTest
            };

            _client = new CoinprismClient(settings);
        }
         
        public async Task<Asset> GetAsset(string id)
        {
            var a = await _client.GetAssetDataAsync(id);

            var asset = new Asset()
            {
                Id = a.AssetId,
                MetadataUrl = a.MetadataUrl,
                FinalMetadataUrl = a.FinalMetadataUrl,
                VerifiedIssuer = a.VerifiedIssuer,
                Name = a.Name,
                ContractUrl = a.ContractUrl,
                NameShort = a.NameShort, 
                Issuer = a.Issuer,
                Description = a.Description,
                DescriptionMime = a.DescriptionMime,
                Type = a.Type,
                Divisibility = a.Divisibility,
                IconUrl = a.IconUrl,
                ImageUrl = a.ImageUrl 
            };

            return asset;
        }

        public async Task<AssetOwners> GetAssetOwners(string id)
        {
            var o = await _client.GetAssetOwnersDataAsync(id);

            var owners = o.Owners.Select(x => new Owner()
            {
                Address = x.Address,
                Quantity = x.AssetQuantity
            }).ToList(); 

            var assetOwners = new AssetOwners()
            {
                BlockHeight = o.BlockHeight,
                Owners = owners
            }; 

            return assetOwners;
        }
    }
}