using System.Threading.Tasks;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{
    public class AssetsEntity : TableEntity, IAssets
    {
        public string AssetId => RowKey;
        public string MetadataUrl { get; set; }
        public string FinalMetadataUrl { get; set; }
        public bool VerifiedIssuer { get; set; }
        public string Name { get; set; }
        public string ContractUrl { get; set; }
        public string NameShort { get; set; }
        public string Issuer { get; set; }
        public string Description { get; set; }
        public string DescriptionMime { get; set; }
        public string Type { get; set; }
        public int Divisibility { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Asset";
        }

        public static string GenerateRowKey(string assetId)
        {
            return assetId;
        }

        public static AssetsEntity CreateNew(IAssets assets)
        {
            return new AssetsEntity
            {
                ContractUrl = assets.ContractUrl,
                Description = assets.Description,
                DescriptionMime = assets.DescriptionMime,
                Divisibility = assets.Divisibility,
                FinalMetadataUrl = assets.FinalMetadataUrl,
                IconUrl = assets.IconUrl,
                ImageUrl = assets.ImageUrl,
                Issuer = assets.Issuer,
                MetadataUrl = assets.MetadataUrl,
                Name = assets.Name,
                NameShort = assets.NameShort,
                Type = assets.Type,
                VerifiedIssuer = assets.VerifiedIssuer,
                PartitionKey = GeneratePartiteonKey(),
                RowKey = GenerateRowKey(assets.AssetId)
            };
        }

    }

    public class AssetsRepository : IAssetsRepository
    {
        private readonly IAzureTableStorage<AssetsEntity> _tableStorage;

        public AssetsRepository(IAzureTableStorage<AssetsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IAssets> GetAssetsDataAsync(string assetId)
        {
            var partiteonKey = AssetsEntity.GeneratePartiteonKey();
            var rowKey = AssetsEntity.GenerateRowKey(assetId);
            var assetData =  await _tableStorage.GetDataAsync(partiteonKey, rowKey);
            return assetData;
        }

        public Task WriteAssetsDataAsync(IAssets assetData)
        {
            var newAssetData = AssetsEntity.CreateNew(assetData);
            return _tableStorage.InsertOrReplaceAsync(newAssetData);
        }
    }
}