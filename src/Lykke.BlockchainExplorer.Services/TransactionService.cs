using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Services
{
    public class TransactionService : ITransactionService
    {
        public IBlockchainDataProvider BlockchainDataProvider { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }
        public IAssetRepository AssetRepository { get; set; }
        public IBlockService BlockService { get; set; }
        public ILog Log { get; set; }

        public TransactionService(IBlockchainDataProvider blockchainProvider,
                            ITransactionRepository transactionRepository,
                            IAssetRepository assetRepository,
                            IBlockService blockService,
                            ILog log)
        {
            BlockchainDataProvider = blockchainProvider;
            TransactionRepository = transactionRepository;
            AssetRepository = assetRepository;
            BlockService = blockService;
            Log = log; 
        }

        public async Task<Transaction> GetTransaction(string id)
        {
            Transaction transaction;

            try 
            {
                transaction = await TransactionRepository.GetById(id);

                if(transaction == null)
                {
                    transaction = await BlockchainDataProvider.GetTransaction(id);
                    
                    if(transaction != null)
                    {
                        await BlockService.GetBlock(transaction.Blockhash);

                        foreach (var asset in transaction.Assets)
                        {
                            var assetExists = await AssetRepository.Exists(asset.Id);

                            if (!assetExists && !String.IsNullOrWhiteSpace(asset.MetadataUrl))
                            {
                                var assetData = await BlockchainDataProvider.GetMetadataContent(new Uri(asset.MetadataUrl));

                                await AssetRepository.Save(assetData);
                            }
                        }

                        await TransactionRepository.Save(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetTransaction", $"transaction_id:{id}", ex, DateTime.Now);
                return null;
            }
             
            return transaction;
        }
    }
}