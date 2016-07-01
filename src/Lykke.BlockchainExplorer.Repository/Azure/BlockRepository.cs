using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Repository.AzureTables;
using Lykke.BlockchainExplorer.Repository.AzureEntities;
using Newtonsoft.Json;
using Lykke.BlockchainExplorer.Settings;
using Lykke.BlockchainExplorer.Core.Utils;
using Lykke.BlockchainExplorer.Repository.AzureBlobs;

namespace Lykke.BlockchainExplorer.Repository.Azure
{
    public class BlockRepository : IBlockRepository
    {
        private const string TableName = "Block";
        private const string BlobContainer = "Block";

        private IAzureTableStorage<BlockEntity> _blockTable;
        private IAzureBlob _blobStorage;
        private IDictionary<string, bool> _importedMap;

        public BlockRepository()
        {
            var connectionString = AppSettings.AzureStorageConnectionString;
            _blockTable = new AzureTableStorage<BlockEntity>(connectionString, TableName, null);
            _blobStorage = new AzureBlobStorage(connectionString);
            _importedMap = new Dictionary<string, bool>();
        }

        public async Task<Block> GetById(string id)
        {
            var rowKey = BlockEntity.GenerateRowKey(id);
            var partitionKey = BlockEntity.GeneratePartiteonKey();

            var blockRecord = await _blockTable.GetDataAsync(partitionKey, rowKey);

            if (blockRecord == null) return null;

            var blockJsonData = await _blobStorage.GetAsync(BlobContainer, blockRecord.Hash);
            var blockJson = Encoding.UTF8.GetString(blockJsonData.ToBytes());
            
            var block = JsonConvert.DeserializeObject<Block>(blockJson);

            _importedMap[id] = blockRecord.IsImported;

            return block;
        }

        public bool IsImported(string id)
        {
            return _importedMap[id];
        }

        public async Task Save(Block entity)
        {
            await SaveEntity(entity);
        }

        public async Task SaveAsImport(Block entity)
        {
            await SaveEntity(entity, isImported: true);
        }

        private async Task SaveEntity(Block entity, bool isImported = false)
        {
            var blockJson = JsonConvert.SerializeObject(entity);
            var blobData = Encoding.UTF8.GetBytes(blockJson);

            await _blobStorage.SaveBlobAsync(BlobContainer, entity.Hash, blobData);

            var blockRecord = BlockEntity.Create(entity.Hash, isImported);

            await _blockTable.InsertOrReplaceAsync(blockRecord);
        }

        Task<bool> IBlockRepository.IsImported(string id)
        {
            throw new NotImplementedException();
        }

        public Task SetAsImported(string id)
        {
            throw new NotImplementedException();
        }
    }
}