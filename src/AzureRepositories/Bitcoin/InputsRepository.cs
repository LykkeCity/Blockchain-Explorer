using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{
    public class InputEntity : TableEntity, IInput
    {
        public double Value { get; set; }
        public string Addresses => RowKey;
        public string Txid { get; set; }
        public string BlockHash => PartitionKey;

        public static string GeneratePartiteonKey(string blockHash)
        {
            return blockHash;
        }

        public static string GenerateRowKey(string adress)
        {
            return adress;
        }

        public static InputEntity CreateNew(IInput input)
        {
            var result = new InputEntity
            {
                Txid = input.Txid,
                Value = input.Value,
                RowKey = GenerateRowKey(input.Addresses),
                PartitionKey = GeneratePartiteonKey(input.BlockHash)
            };

            return result;
        }
    }

    public class InputsRepository : IInputsRepository
    {
        private readonly IAzureTableStorage<InputEntity> _tableStorage;

        public InputsRepository(IAzureTableStorage<InputEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(Input input)
        {
            var newInputs = InputEntity.CreateNew(input);
            return _tableStorage.InsertOrReplaceAsync(newInputs);
        }

        public async Task<IEnumerable<IInput>> GetAsync(string hashBlock)
        {
            var partitionKey = InputEntity.GeneratePartiteonKey(hashBlock);
            return await _tableStorage.GetDataAsync(partitionKey);

        }
    }
}