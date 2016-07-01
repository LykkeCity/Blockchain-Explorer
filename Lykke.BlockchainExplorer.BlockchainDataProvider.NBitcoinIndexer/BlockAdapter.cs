using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using NBitcoin.Indexer;
using NBitcoin;
using Newtonsoft.Json;
using Lykke.BlockchainExplorer.BlockchainDataProvider.NBitcoinIndexer.Model;
using NBitcoin.OpenAsset;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.NBitcoinIndexer
{
    public class BlockAdapter : IBlockProvider
    {
        private IndexerClient _client;
        
        public BlockAdapter()
        {
            var configuration = IndexerConfiguration.FromConfiguration();
            _client = new IndexerClient(configuration);
        }

        private static ConcurrentChain _chain;

        public async Task<Core.Domain.Block> GetBlock(string id)
        {
            var blockId = NBitcoin.uint256.Parse(id);
            
            var blockRecord = await Task.Run<NBitcoin.Block>(() => _client.GetBlock(blockId));

            ConcurrentChain chain;

            if(_chain == null)
            {
                _chain = _client.GetMainChain();
            }

            chain = _chain;

            var confirmed = chain.GetBlock(blockId);

            if (blockRecord == null) return null;

            var blockInformation = new BlockInformation
            {
                BlockId = confirmed.HashBlock,
                BlockHeader = confirmed.Header,
                Confirmations = chain.Tip.Height - confirmed.Height + 1,
                Height = confirmed.Height,
                MedianTimePast = confirmed.GetMedianTimePast(),
                BlockTime = confirmed.Header.BlockTime
            };

            var transactionList = new List<Core.Domain.Transaction>();

            foreach (var t in blockRecord.Transactions)
            {
                var transInfo = _client.GetTransaction(t.GetHash());
                var colored = NBitcoin.OpenAsset.Extensions.HasValidColoredMarker(t);
                var tranExtraInfo = GetTransactionInfo(transInfo, colored, blockInformation);

                var transJson = Serializer.ToString<GetTransactionResponse>(tranExtraInfo, NBitcoin.Network.TestNet);
                var model = Serializer.ToObject<TransactionNinjaModel>(transJson, NBitcoin.Network.TestNet);

                if(colored)
                {
                    var colorMarker = ColorMarker.Get(t);
                    model.TransactionUrl = colorMarker.GetMetadataUrl();
                }

                model.CalculateInputsWithReturnedChange();

                var transaction = GetTransaction(model);

                transactionList.Add(transaction);
            }
            
            var block = new Core.Domain.Block()
            {
                Hash = id,
                Height = confirmed.Height,
                Time = confirmed.Header.BlockTime.UtcDateTime,
                Confirmations = chain.Tip.Height - confirmed.Height + 1,
                Difficulty = confirmed.Header.Bits.Difficulty,
                MerkleRoot = confirmed.Header.HashMerkleRoot.ToString(),
                Nonce = confirmed.Header.Nonce,
                PreviousBlock = confirmed.Header.HashPrevBlock.ToString(),
                Transactions = transactionList,
                TotalTransactions = transactionList.Count
            };

            return block;
        }

        private GetTransactionResponse GetTransactionInfo(TransactionEntry tx, bool colored, BlockInformation block)
        {
            var response = new GetTransactionResponse()
            {
                TransactionId = tx.TransactionId,
                Transaction = tx.Transaction,
                IsCoinbase = tx.Transaction.IsCoinBase,
                Fees = tx.Fees,
                Block = block,
                FirstSeen = tx.FirstSeen
            };
            for (int i = 0; i < tx.Transaction.Outputs.Count; i++)
            {
                var txout = tx.Transaction.Outputs[i];
                NBitcoin.ICoin coin = new NBitcoin.Coin(new NBitcoin.OutPoint(tx.TransactionId, i), txout);
                if (colored)
                {
                    var entry = tx.ColoredTransaction.GetColoredEntry((uint)i);
                    if (entry != null)
                        coin = new NBitcoin.ColoredCoin(entry.Asset, (NBitcoin.Coin)coin);
                }
                response.ReceivedCoins.Add(coin);
            }
            if (!response.IsCoinbase)
                for (int i = 0; i < tx.Transaction.Inputs.Count; i++)
                {
                    NBitcoin.ICoin coin = new NBitcoin.Coin(tx.SpentCoins[i].OutPoint, tx.SpentCoins[i].TxOut);
                    if (colored)
                    {
                        var entry = tx.ColoredTransaction.Inputs.FirstOrDefault(ii => ii.Index == i);
                        if (entry != null)
                            coin = new NBitcoin.ColoredCoin(entry.Asset, (NBitcoin.Coin)coin);
                    }
                    response.SpentCoins.Add(coin);
                }

            return response;
        }

        private Core.Domain.Transaction GetTransaction(TransactionNinjaModel t)
        {
            var block = new Core.Domain.Block()
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
                if(t.TransactionUrl != null)
                {
                    asset.MetadataUrl = t.TransactionUrl.ToString();
                }

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

            var transaction = new Core.Domain.Transaction()
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

        public async Task<Core.Domain.Block> GetLastBlock()
        {
            var blockRecord = await Task.Run<ChainBlockHeader>(() => _client.GetBestBlock());

            var block = new Core.Domain.Block()
            {
                Hash = blockRecord.BlockId.ToString(),
                Height = blockRecord.Height,
            };

            return block;
        }
    }
}
