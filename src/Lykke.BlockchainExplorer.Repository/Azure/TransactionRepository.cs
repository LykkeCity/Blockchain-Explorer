using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Repository.AzureTables;
using Lykke.BlockchainExplorer.Repository.AzureEntities;
using Lykke.BlockchainExplorer.Settings;
using Newtonsoft.Json;

namespace Lykke.BlockchainExplorer.Repository.Azure
{
    public class TransactionRepository : ITransactionRepository
    {
        private const string TableName = "Transaction";
        private IAzureTableStorage<TransactionEntity> transactionStorage;

        public TransactionRepository()
        {
            var connectionString = AppSettings.AzureStorageConnectionString;
            transactionStorage = new AzureTableStorage<TransactionEntity>(connectionString, TableName, null);
        } 

        public async Task<Transaction> GetById(string id)
        {
            var rowKey = TransactionEntity.GenerateRowKey(id);
            var partitionKey = TransactionEntity.GeneratePartiteonKey();

            var transactionRecord = await transactionStorage.GetDataAsync(partitionKey, rowKey);

            if (transactionRecord == null) return null;

            var block = JsonConvert.DeserializeObject<Transaction>(transactionRecord.JsonData);

            return block;
        }

        public Task<bool> IsImported(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Save(Transaction entity)
        {
            await SaveEntity(entity);
        }

        public async Task SaveAsImport(Transaction entity)
        {
            await SaveEntity(entity, isImported: true);
        }

        public Task SetAsImported(string id)
        {
            throw new NotImplementedException();
        }

        private async Task SaveEntity(Transaction entity, bool isImported = false)
        {
            var transactionJson = JsonConvert.SerializeObject(entity);

            var transactionEntity = TransactionEntity.Create(entity.TransactionId, transactionJson, isImported);

            await transactionStorage.InsertOrReplaceAsync(transactionEntity);
        } 
    }
}
