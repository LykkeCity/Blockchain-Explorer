using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureStorage.Blobs;
using AzureStorage.Tables;
using Common;
using Common.Json;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureRepositories.Bitcoin
{
    public class AssetsOwnersEntity : TableEntity, IAssetsOwners
    {
        public long BlockHeight { get; set; }
        public string AssetId => RowKey;
        public string Name { get; set; }
        public IOwner[] Owners { get; set; }

        public string BlobId { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "AssetsOwner";
        }

        public static string GenerateRowKey(string assetId)
        {
            return assetId;
        }


        public static AssetsOwnersEntity CreateNew(IAssetsOwners assetsOwners, string blobId)
        {
            return new AssetsOwnersEntity
            {
                RowKey = GenerateRowKey(assetsOwners.AssetId),
                PartitionKey = GeneratePartiteonKey(),
                BlobId = blobId,
                BlockHeight = assetsOwners.BlockHeight
            };
        }
    }

    public class AssetsOwnersRepository : IAssetsOwnersRepository
    {
        private readonly IAzureTableStorage<AssetsOwnersEntity> _tableStorage;
        private readonly IAzureBlob _azureBlob;

        public const string BlobContainer = "assetsowners";

        public AssetsOwnersRepository(IAzureTableStorage<AssetsOwnersEntity> tableStorage, IAzureBlob azureBlob)
        {
            _tableStorage = tableStorage;
            _azureBlob = azureBlob;
        }


        public async Task<IAssetsOwners> GetAssetsOwnersDataAsync(string assetId)
        {
            var rowKey = AssetsOwnersEntity.GenerateRowKey(assetId);
            var partiteonKey = AssetsOwnersEntity.GeneratePartiteonKey();
            var assetData = await _tableStorage.GetDataAsync(partiteonKey, rowKey);
            if (assetData != null)
            {
                var blobAssetData = await _azureBlob.GetAsync(BlobContainer, assetData.BlobId);
                if (blobAssetData != null)
                {
                    string jsonStr = Encoding.UTF8.GetString(blobAssetData.ToBytes());
                    return JsonConvert.DeserializeObject<DeserializeAssetsOwner>(jsonStr);
                }
            }
            return null;
        }

        public async Task WriteAssetsOwnersDataAsync(IAssetsOwners assetOwnersData)
        {
            var key = Guid.NewGuid().ToString();
            var bytes = assetOwnersData.ToJson().ToUtf8ByteArray();
            await _azureBlob.SaveBlobAsync(BlobContainer, key, bytes);
            await _tableStorage.InsertOrReplaceAsync(AssetsOwnersEntity.CreateNew(assetOwnersData, key));
        }
    }
}