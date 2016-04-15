using System;
using System.Text;
using System.Threading.Tasks;
using AzureStorage.Blobs;
using AzureStorage.Tables;
using Common;
using Common.Json;
using Core.BitcoinNinja;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureRepositories.BitcoinNinja
{

    public class BlockNinjaEntity : TableEntity
    {
        public string Hash => RowKey;

        public string BlobId { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Block";
        }
        
        public static string GenerateRowKey(string hash)
        {
            return hash;
        }

        public static BlockNinjaEntity Create(string hash, string blobId)
        {
            return new BlockNinjaEntity
            {
                BlobId = blobId,
                RowKey = GenerateRowKey(hash),
                PartitionKey = GeneratePartiteonKey()
            };
        }

    }

    public class BlockRepository : IBlockNinjaRepository
    {
        private readonly IAzureTableStorage<BlockNinjaEntity> _tableStorage;
        private readonly IAzureBlob _azureBlob;

        public const string BlobContainer = "blocksninja";

        public BlockRepository(IAzureTableStorage<BlockNinjaEntity> tableStorage, IAzureBlob azureBlob)
        {
            _tableStorage = tableStorage;
            _azureBlob = azureBlob;
        }

        public async Task<IBlockNinja> GetBlockDataAsync(string blockId)
        {
            var rowKey = BlockNinjaEntity.GenerateRowKey(blockId);
            var partiteonKey = BlockNinjaEntity.GeneratePartiteonKey();
            var block = await _tableStorage.GetDataAsync(partiteonKey, rowKey);
            if (block == null) return null;
            var blobBlockData = await _azureBlob.GetAsync(BlobContainer, block.BlobId);
            var jsonStr = Encoding.UTF8.GetString(blobBlockData.ToBytes());
            return JsonConvert.DeserializeObject<DeserializeBlockNinja>(jsonStr);
        }

        public async Task WriteBlockDataAsync(IBlockNinja blockData)
        {
            var key = Guid.NewGuid().ToString();
            var bytes = blockData.ToJson().ToUtf8ByteArray();
            await _azureBlob.SaveBlobAsync(BlobContainer, key, bytes);
            await _tableStorage.InsertOrReplaceAsync(BlockNinjaEntity.Create(blockData.Hash, key));
        }
    }
}