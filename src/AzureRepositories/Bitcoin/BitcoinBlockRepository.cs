using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Core.Bitcoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Bitcoin
{

    public class BicoinBlockEntity : TableEntity, IBitcoinBlock
    {
        public string Hash => RowKey;
        public DateTime Time { get; set; }
        public long Height { get; set; }
        public string Previousblockhash { get; set; }
        public string Nextblockhash { get; set; }
        public double Difficulty { get; set; }
        public string Merkleroot { get; set; }
        public long Nonce { get; set; }
        public long TotalTransactions { get; set; }

        public static string GeneratePartiteonKey()
        {
            return "Bitcoin";
        }

        public static string GenerateRowKey(string hash)
        {
            return hash;
        }


        public static BicoinBlockEntity CreateNew(IBitcoinBlock src)
        {
            var result = new BicoinBlockEntity
            {
                PartitionKey = GeneratePartiteonKey(),
                RowKey = GenerateRowKey(src.Hash),
                Time = src.Time,
                Height = src.Height,
                Difficulty = src.Difficulty,
                Merkleroot = src.Merkleroot,
                Nextblockhash = src.Nextblockhash,
                Nonce = src.Nonce,
                Previousblockhash = src.Previousblockhash,
                TotalTransactions = src.TotalTransactions
            };

            return result;
        }


    }

    public class BitcoinBlockRepository : IBitcoinBlockRepository
    {
        private readonly IAzureTableStorage<BicoinBlockEntity> _tableStorage;

        public BitcoinBlockRepository(IAzureTableStorage<BicoinBlockEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(IBitcoinBlock bitcoinBlock)
        {
            var newEntity = BicoinBlockEntity.CreateNew(bitcoinBlock);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<IBitcoinBlock> GetAsync(string block)
        {
            var partitionKey = BicoinBlockEntity.GeneratePartiteonKey();
            var rowKey = BicoinBlockEntity.GenerateRowKey(block);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}