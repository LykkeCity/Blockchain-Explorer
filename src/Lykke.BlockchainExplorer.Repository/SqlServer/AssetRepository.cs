using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;

namespace Lykke.BlockchainExplorer.Repository.SqlServer
{
    public class AssetRepository : IAssetRepository, IDisposable
    {
        private Orm.Entities _context;

        public AssetRepository()
        {
            _context = new Orm.Entities();
        }

        public async Task<bool> Exists(string id)
        {
            return await Task.Run<bool>(() => 
            {
                return AssetExists(id);
            });
        }

        private bool AssetExists(string id)
        {
            var res = _context.AssetExists(id).FirstOrDefault();

            if (res == null) return false;
            return res.GetValueOrDefault();
        }

        public async Task<Asset> GetById(string id)
        {
            return await Task.Run<Asset>(() =>
            {
                return GetAssetById(id);
            }); 
        }

        private Asset GetAssetById(string id)
        {
            Asset asset = null;

            var assetRecord = _context.GetAssetById(id).FirstOrDefault();

            if(assetRecord != null)
            {
                asset = new Asset()
                {
                    Id = assetRecord.Hash,
                    Name = assetRecord.Name, 
                    NameShort = assetRecord.NameShort,
                    Description = assetRecord.Description,
                    DescriptionMime = assetRecord.DescriptionMime,
                    Type = assetRecord.Type,
                    ContractUrl = assetRecord.ContractUrl,
                    MetadataUrl = assetRecord.MetadataUrl,
                    FinalMetadataUrl = assetRecord.FinalMetadataUrl,
                    Issuer = assetRecord.Issuer,
                    VerifiedIssuer = assetRecord.VerifiedIssuer.GetValueOrDefault(),
                    Divisibility = assetRecord.Divisibility.GetValueOrDefault(),
                    IconUrl = assetRecord.IconUrl,
                    ImageUrl = assetRecord.ImageUrl,
                    Version = assetRecord.Version
                };
            }

            return asset;
        }

        public async Task Save(Asset entity)
        {
            await Task.Run(() =>
            {
                SaveAsset(entity);
            });
        }

        private void SaveAsset(Asset entity)
        {
            _context.InsertAsset(entity.Id, entity.Name, entity.NameShort, entity.Description, entity.DescriptionMime,
                                 entity.Type, entity.ContractUrl, entity.MetadataUrl, entity.FinalMetadataUrl, entity.Issuer,
                                 entity.VerifiedIssuer, entity.Divisibility, entity.IconUrl, entity.ImageUrl, entity.Version);
        }

        public async Task<IList<Asset>> GetAssets()
        { 
            return await Task.Run<IList<Asset>>(() =>
            {
                return GetAssetList();
            });
        }

        private IList<Asset> GetAssetList()
        {
            var assets = _context.GetAssets()
                                 .Select(x => new Asset()
                                 {
                                     Id = x.Hash,
                                     MetadataUrl = x.MetadataUrl,
                                     FinalMetadataUrl = x.FinalMetadataUrl,
                                     VerifiedIssuer = x.VerifiedIssuer.GetValueOrDefault(),
                                     Name = x.Name,
                                     NameShort = x.NameShort,
                                     ContractUrl = x.ContractUrl,
                                     Issuer = x.Issuer,
                                     Description = x.Description,  
                                     DescriptionMime = x.DescriptionMime,
                                     Type = x.Type,
                                     Divisibility = x.Divisibility.GetValueOrDefault(),
                                     IconUrl = x.IconUrl,
                                     ImageUrl = x.ImageUrl,
                                     Version = x.Version
                                 }).ToList();

            return assets;
        }

        public async Task<AssetOwners> GetAssetOwners(string id)
        {
            return await Task.Run<AssetOwners>(() =>
            {
                return GetAssetOwnerList(id);
            });
        }

        private AssetOwners GetAssetOwnerList(string id)
        {
            var asset = GetAssetById(id);

            var owners = _context.GetAssetOwners(id) 
                                 .Select(x => new Owner()
                                 {
                                     Address = x.Address,
                                     Quantity = x.Quantity.GetValueOrDefault()
                                 }).ToList();

            var assetOwners = new AssetOwners()
            {
                Asset = asset,
                Owners = owners  
            };

            return assetOwners;
        }
         
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
