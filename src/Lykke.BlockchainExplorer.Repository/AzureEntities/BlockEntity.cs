using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.AzureEntities
{
    internal class BlockEntity : TableEntity
    {
        public string Hash => RowKey;

        public bool IsImported { get; set; }

        public string BlobId { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Block";
        }

        public static string GenerateRowKey(string hash)
        {
            return hash;
        } 

        public static BlockEntity Create(string hash, bool isImported = false)
        {
            return new BlockEntity
            {
                RowKey = GenerateRowKey(hash),
                PartitionKey = GeneratePartiteonKey(),
                IsImported = isImported,
                BlobId = hash
            };
        }
    }
}