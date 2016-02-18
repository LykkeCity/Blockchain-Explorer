using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{

    public class TransactionEntity : TableEntity, ITransaction
    {
        public string Txid => RowKey;
        public string Blockhash { get; set; }
        public long Confirmations { get; set; }
        public DateTime Time { get; set; }
        public long Height { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Transaction";
        }

        public static string GenerateRowKey(string txid)
        {
            return txid;
        }

        public static TransactionEntity CreateNew(ITransaction transaction)
        {
            var result = new TransactionEntity
            {
                Time = transaction.Time,
                Confirmations = transaction.Confirmations,
                Height = transaction.Height,
                Blockhash = transaction.Blockhash,
                RowKey = GenerateRowKey(transaction.Txid),
                PartitionKey = GeneratePartiteonKey()
            };

            return result;
        }


    }

    public class TransactionRepository:ITransactionRepository
    {
        private readonly IAzureTableStorage<TransactionEntity> _tableStorage;

        public TransactionRepository(IAzureTableStorage<TransactionEntity> tableStorage )
        {
            _tableStorage = tableStorage;
        }

        public  Task SaveAsync(Transaction transaction)
        {
            var newEntity = TransactionEntity.CreateNew(transaction);
            return  _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<IEnumerable<ITransaction>> GetAsync(string hashBlock)
        {
            var partitionKey = TransactionEntity.GeneratePartiteonKey();
            return await _tableStorage.GetDataAsync(partitionKey);

        }

        public async Task<ITransaction> GetTransaction(string tx)
        {
            var rowKey = TransactionEntity.GenerateRowKey(tx);
            var partitionKey = TransactionEntity.GeneratePartiteonKey();
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}