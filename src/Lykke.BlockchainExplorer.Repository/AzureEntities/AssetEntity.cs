using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.AzureEntities
{
    internal class AssetEntity : TableEntity
    {
        public string Hash => RowKey;

        public string AssetName { get; set; }

        public string JsonData { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Asset";
        } 

        public static string GenerateRowKey(string hash)
        {
            return hash;
        } 

        public static AssetEntity Create(string hash, string blobId)
        {
            return new AssetEntity
            {
                RowKey = GenerateRowKey(hash),
                PartitionKey = GeneratePartiteonKey()
            };
        }
    }
}