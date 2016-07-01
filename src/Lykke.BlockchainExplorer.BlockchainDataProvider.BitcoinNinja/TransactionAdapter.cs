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
    public class TransactionAdapter : ITransactionProvider
    {
        private BitcoinNinjaClient _client;

        public TransactionAdapter()
        {
            var settings = new BitcoinNinjaSettings()
            {
                Network = AppSettings.Network,
                UrlMain = AppSettings.SqlNinjaUrlMain,
                UrlTest = AppSettings.SqlNinjaUrlTest
            };

            _client = new BitcoinNinjaClient(settings);
        }

        public async Task<Transaction> GetTransaction(string id)
        {
            var t = await _client.GetTransactionAsync(id); 

            var block = new Block()
            {
                Hash = t.Block.Hash,
                Height = t.Block.Height,
                Time = t.Block.Time,
                Confirmations = t.Block.Confirmations
            };

            var inputs = t.DeserializeInputs.Select(x => new In()
            { 
                TransactionId = x.TxId,
                Address = x.Address,
                Index = x.Index,
                Value = x.Value,
                AssetId = x.AssetId,
                Quantity = x.Quantity
            }).ToList();

            var outputs = t.DeserializeOutputs.Select(x => new Out()
            {
                TransactionId = x.TxId,
                Address = x.Address,
                Index = x.Index,
                Value = x.Value, 
                AssetId = x.AssetId,
                Quantity = x.Quantity
            }).ToList();

            var assets = new List<Core.Domain.Asset>();

            foreach (var k in t.Asset.Keys)
            {
                var assetRecord = t.Asset[k]; 

                var asset = new Core.Domain.Asset();
                asset.Id = k;
                asset.MetadataUrl = t.TransactionUrl.ToString(); 

                asset.Input = assetRecord.AssetDataInput.Select(x => new In()
                {
                    TransactionId = x.TxId,
                    Address = x.Address,
                    Index = x.Index,
                    Value = x.Value,
                    AssetId = x.AssetId,
                    Quantity = x.Quantity  
                }).ToList();

                asset.Output = assetRecord.AssetDataOutput.Select(x => new Out()
                {
                    TransactionId = x.TxId,
                    Address = x.Address,
                    Index = x.Index, 
                    Value = x.Value,
                    AssetId = x.AssetId,
                    Quantity = x.Quantity
                }).ToList();

                assets.Add(asset);
            }

            var transaction = new Transaction()
            {
                TransactionId = t.TxId,
                Time = block.Time,
                Hex = t.Hex,
                Fees = t.Fees,
                IsCoinBase = t.IsCoinBase,
                IsColor = t.IsColor,
                Block = block,
                Blockhash = t.Block.Hash,
                TransactionIn = inputs,
                TransactionsOut = outputs, 
                Assets = assets
            };

            return transaction;
        }
    }
}