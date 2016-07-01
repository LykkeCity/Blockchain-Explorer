using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BitcoinBridge.Jobs
{
    public class BlockTransfer : IBlockTransfer
    {
        private IBlockRepository _blockRepository;
        private ITransactionRepository _transactionRepository;
        private IBlockchainDataProvider _dataProvider;
        private IAssetRepository _assetRepository;
        private ILog _log;

        private string _nextBlockHash;

        public BlockTransfer(IBlockRepository blockRepository,
                             ITransactionRepository transactionRepository,
                             IAssetRepository assetRepository,
                             IBlockchainDataProvider dataProvider,
                             ILog log)
        {
            _blockRepository = blockRepository;
            _transactionRepository = transactionRepository;
            _dataProvider = dataProvider;
            _assetRepository = assetRepository;
            _log = log;
        }

        public async Task Start()
        {
            await ImportBlocks(); 
        }

        private async Task ImportBlocks()
        {
            var lastBlock = await _dataProvider.GetLastBlock();
            _nextBlockHash = lastBlock.Hash;

            Block block;

            do
            {
                try
                {
                    block = await _dataProvider.GetBlock(_nextBlockHash);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error on block {_nextBlockHash}");
                    break;
                }
                

                var blockRecord = await _blockRepository.GetById(block.Hash);
                var isBlockImported = await _blockRepository.IsImported(block.Hash);

                if (blockRecord != null && isBlockImported)
                {
                    _nextBlockHash = block.PreviousBlock;
                    continue;
                }
                else if(blockRecord != null && !isBlockImported)
                {
                    await _blockRepository.SetAsImported(blockRecord.Hash);
                }
                else
                {
                    await _blockRepository.SaveAsImport(block); 
                }

                foreach (var t in block.Transactions)
                {
                    var transactionRecord = await _transactionRepository.GetById(t.TransactionId);

                    if(transactionRecord == null)
                    {
                        //var transaction = await _dataProvider.GetTransaction(t.TransactionId);
                        var transaction = t;

                        foreach (var asset in transaction.Assets)
                        {
                            var assetExists = await _assetRepository.Exists(asset.Id);

                            if (!assetExists && !String.IsNullOrWhiteSpace(asset.MetadataUrl))
                            {
                                var assetData = await _dataProvider.GetMetadataContent(new Uri(asset.MetadataUrl));

                                await _assetRepository.Save(assetData);
                            }
                        }

                        await _transactionRepository.SaveAsImport(transaction);
                    }
                    else
                    {
                        await _transactionRepository.SetAsImported(transactionRecord.TransactionId);
                    }
                }

                _nextBlockHash = block.PreviousBlock;
                
                Console.WriteLine($"Imported block {block.Hash} with {block.Transactions.Count} transactions");
            }
            while (!String.IsNullOrWhiteSpace(block.PreviousBlock));

            Console.WriteLine("Import finished");
        }
    }
}
