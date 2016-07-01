using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.AzureEntities
{
    internal class TransactionEntity : TableEntity
    {
        public string Hash => RowKey;

        public string JsonData { get; set; }

        public bool IsImported { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Transaction";
        }

        public static string GenerateRowKey(string hash)
        {
            return hash;
        }

        public static TransactionEntity Create(string hash, string jsonData, bool isImported = false)
        {
            return new TransactionEntity
            {
                RowKey = GenerateRowKey(hash),
                PartitionKey = GeneratePartiteonKey(),
                JsonData = jsonData,
                IsImported = isImported
            };
        }
    }
}