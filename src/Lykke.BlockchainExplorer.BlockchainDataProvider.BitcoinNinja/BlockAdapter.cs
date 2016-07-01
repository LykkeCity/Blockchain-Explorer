using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja.Client;
using Lykke.BlockchainExplorer.Settings;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja
{
    public class BlockAdapter : IBlockProvider
    {
        private BitcoinNinjaClient _client;

        public BlockAdapter()
        {
            var settings = new BitcoinNinjaSettings()
            {
                Network = AppSettings.Network,
                UrlMain = AppSettings.SqlNinjaUrlMain,
                UrlTest = AppSettings.SqlNinjaUrlTest
            };

            _client = new BitcoinNinjaClient(settings);
        }

        public async Task<Block> GetBlock(string id)
        {
            var b = await _client.GetInformationBlockAsync(id);

            var transactions = b.Transactions.Select(x => new Transaction()
            {
                TransactionId = x.TxId,
                IsColor = x.IsColor
            }).ToList();
             
            var block = new Block()
            {
                Hash = b.Hash,
                Height = b.Height,
                Time = b.Time,
                Confirmations = b.Confirmations,
                Difficulty = b.Difficulty,
                MerkleRoot = b.MerkleRoot,
                Nonce = b.Nonce,
                TotalTransactions = b.TotalTransactions, 
                PreviousBlock = b.PreviousBlock,
                Transactions = transactions
            };
             
            return block;
        }
         
        public async Task<Block> GetLastBlock()
        {
            var b = await _client.GetLastBlockAsync();

            var block = new Block()
            {
                Hash = b.Hash,
                Height = b.Height
            };

            return block;
        }
    }
}