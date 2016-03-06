using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common;
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

        public TransactionRepository(IAzureTableStorage<TransactionEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task SaveAsync(Transaction transaction)
        {
            var newEntity = TransactionEntity.CreateNew(transaction);
            await _tableStorage.InsertOrReplaceAsync(newEntity);
            //Добавляем еще одну запись но с другим PartitionKey
            newEntity.PartitionKey = newEntity.Blockhash;
            await _tableStorage.InsertOrReplaceAsync(newEntity);

        }

        public async Task<IEnumerable<ITransaction>> GetAsync(string hashBlock, int page, int count)
        {
            var partitionKey = hashBlock;           
            return await _tableStorage.TakePageAsync(partitionKey, page, count);

        }

       /* public async Task UpdateIndexec()
        {
            var partitionKey = TransactionEntity.GeneratePartiteonKey();
            var tableQuery =
                TableStorageUtils.QueryGenerator<TransactionEntity>.PartitionKeyOnly.GetTableQuery(partitionKey);
            await _tableStorage.ExecuteQueryAsync(tableQuery, chank =>
            {
                foreach (var items in chank.ToPieces(100))
                {
                    var newEntits = items.Select(itm => AzureIndex.Create(IndexByBlockHash, itm.Blockhash, itm)).ToArray();
                    _azureIndex.InsertOrReplaceBatchAsync(newEntits).Wait();
                    Console.WriteLine("Add new"+ newEntits.Length + "items");
                }
            });
        }*/



        public async Task<ITransaction> GetTransaction(string tx)
        {
            var rowKey = TransactionEntity.GenerateRowKey(tx);
            var partitionKey = TransactionEntity.GeneratePartiteonKey();
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}