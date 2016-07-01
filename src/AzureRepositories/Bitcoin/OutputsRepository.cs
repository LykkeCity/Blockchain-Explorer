using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{

    public class OutputEntity : TableEntity, IOutput
    {
        public double Value { get; set; }
        public string Addresses => RowKey;
        public string BlockHash => PartitionKey;
        public string Txid { get; set; }
        public DateTime Time { get; set; }

        public static string GeneratePartiteonKey(string blockHash)
        {
            return blockHash;
        }

        public static string GenerateRowKey(string adress)
        {
            return adress;
        }

        public static OutputEntity CreateNew(IOutput output)
        {
            var result = new OutputEntity
            {
                Time = output.Time,
                Txid = output.Txid,
                Value = output.Value,
                RowKey = GenerateRowKey(output.Addresses),
                PartitionKey = GeneratePartiteonKey(output.BlockHash)
            };

            return result;
        }
    }

    public class OutputsRepository: IOutputsRepository
    {
        private readonly IAzureTableStorage<OutputEntity> _tableStorage;

        public OutputsRepository(IAzureTableStorage<OutputEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(IOutput output)
        {
            var newOutputs = OutputEntity.CreateNew(output);
            return _tableStorage.InsertOrReplaceAsync(newOutputs);
        }

        public async Task<IEnumerable<IOutput>> GetAsync(string hashBlock)
        {
            var partitionKey = OutputEntity.GeneratePartiteonKey(hashBlock);
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}