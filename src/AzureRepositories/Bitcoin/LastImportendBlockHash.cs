using System.Threading.Tasks;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{
    public class LastImportendBlockHashEntyti : TableEntity, ILastBlock
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

        public static LastImportendBlockHashEntyti Create(string hash, long height)
        {
            return new LastImportendBlockHashEntyti
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
        private readonly IAzureTableStorage<LastImportendBlockHashEntyti> _tableStorage;

        public LastImportendBlockHash(IAzureTableStorage<LastImportendBlockHashEntyti> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<string> GetAsync()
        {
            var partitionKey = LastImportendBlockHashEntyti.GeneratePartitionKey();
            var rowKey = LastImportendBlockHashEntyti.GenerateRowKey();

            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            return entity?.Hash;

        }

        public Task SetAsync(string hash, long height)
        {
            var newEntity = LastImportendBlockHashEntyti.Create(hash, height);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<ILastBlock> GetLastBlock()
        {
            var partitionKey = LastImportendBlockHashEntyti.GeneratePartitionKey();
            var rowKey = LastImportendBlockHashEntyti.GenerateRowKey();

            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            return entity;
        }
    }
}
