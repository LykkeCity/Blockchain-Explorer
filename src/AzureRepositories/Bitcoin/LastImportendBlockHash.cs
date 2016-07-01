using System.Threading.Tasks;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{
    public class LastImportedBlockHashEntity : TableEntity, ILastBlock
    {
        public string Hash { get; set; }
        public long Height { get; set; }

        public static string GeneratePartitionKey()
        {
            return "settings";
        }

        public static string GenerateRowKey()
        {
            return "LastBlockid";
        }

        public static LastImportedBlockHashEntity Create(string hash, long height)
        {
            return new LastImportedBlockHashEntity
            {
                RowKey = GenerateRowKey(),
                PartitionKey = GeneratePartitionKey(),
                Hash = hash,
                Height = height
            };
        }
         
    }

    public class LastImportendBlockHash : ILastImportendBlockHash
    {
        private readonly IAzureTableStorage<LastImportedBlockHashEntity> _tableStorage;

        public LastImportendBlockHash(IAzureTableStorage<LastImportedBlockHashEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<string> GetAsync()
        {
            var partitionKey = LastImportedBlockHashEntity.GeneratePartitionKey();
            var rowKey = LastImportedBlockHashEntity.GenerateRowKey();

            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            return entity?.Hash;

        }

        public Task SetAsync(string hash, long height)
        {
            var newEntity = LastImportedBlockHashEntity.Create(hash, height);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<ILastBlock> GetLastBlock()
        {
            var partitionKey = LastImportedBlockHashEntity.GeneratePartitionKey();
            var rowKey = LastImportedBlockHashEntity.GenerateRowKey();

            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            return entity;
        }
    }
}