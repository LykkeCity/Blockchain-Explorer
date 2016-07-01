using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.AzureEntities
{
    internal class LogEntity : TableEntity
    {
        public string Hash => RowKey;

        public string JsonData { get; set; }

        public static string GeneratePartiteonKey(string errorLevel)
        {
            return errorLevel;
        }

        public static string GenerateRowKey()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static LogEntity Create(string errorLevel, string errorDataJson)
        {
            return new LogEntity
            {
                RowKey = GenerateRowKey(),
                PartitionKey = GeneratePartiteonKey(errorLevel),
                JsonData = errorDataJson
            };
        }
    }
}
