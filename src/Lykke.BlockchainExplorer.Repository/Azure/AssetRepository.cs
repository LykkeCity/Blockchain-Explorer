using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Repository.AzureTables;
using Lykke.BlockchainExplorer.Repository.AzureEntities;
using Lykke.BlockchainExplorer.Settings;
using Newtonsoft.Json;

namespace Lykke.BlockchainExplorer.Repository.Azure
{
    public class AssetRepository : IAssetRepository
    {
        private const string TableName = "Asset";
        private IAzureTableStorage<AssetEntity> _assetTable;

        public AssetRepository()
        {
            var connectionString = AppSettings.AzureStorageConnectionString;
            _assetTable = new AzureTableStorage<AssetEntity>(connectionString, TableName, null);
        }

        public Task<bool> Exists(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AssetOwners> GetAssetOwners(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Asset>> GetAssets()
        {
            throw new NotImplementedException();
        }

        public async Task<Asset> GetById(string id)
        {
            var rowKey = BlockEntity.GenerateRowKey(id);
            var partitionKey = BlockEntity.GeneratePartiteonKey();

            var assetRecord = await _assetTable.GetDataAsync(partitionKey, rowKey);

            if (assetRecord == null) return null;

            return (Asset)JsonConvert.DeserializeObject(assetRecord.JsonData);
        }

        public async Task Save(Asset entity)
        {
            var blockJson = JsonConvert.SerializeObject(entity);

            var blockRecord = new AssetEntity()
            {
                RowKey = entity.Id,
                PartitionKey = AssetEntity.GeneratePartiteonKey(),
                JsonData = blockJson
            };

            await _assetTable.InsertOrReplaceAsync(blockRecord);
        }
    }
}