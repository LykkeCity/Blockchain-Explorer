using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Enums;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Services
{
    public class SearchService : ISearchService
    {
        public IBlockService BlockService { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IAssetService AssetService { get; set; }
        public IAddressService AddressService { get; set; }
        public ILog Log { get; set; }

        public SearchService(IBlockService blockService,
                             ITransactionService transactionService,
                             IAssetService assetService,
                             IAddressService addressService,
                             ILog log) 
        {
            BlockService = blockService;
            TransactionService = transactionService;
            AssetService = assetService;
            AddressService = addressService;
            Log = log;
        }

        public async Task<EntitySearchResult> SearchEntityById(string id)
        {
            Block block;
            Transaction transaction;
            Address address; 
            Asset asset;
            
            try
            {
                block = await BlockService.GetBlock(id);

                if (block != null)
                {
                    return EntitySearchResult.Block;
                } 

                transaction = await TransactionService.GetTransaction(id);

                if (transaction != null)
                {
                    return EntitySearchResult.Transaction;
                }

                address = await AddressService.GetAddress(id);

                if (address != null)
                {
                    return EntitySearchResult.Address;
                }

                asset = await AssetService.GetAsset(id);
                 
                if (asset != null)
                {
                    return EntitySearchResult.Asset;
                }
            }
            catch(Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "SearchEntityById", $"entity_id:{id}", ex, DateTime.Now);
            }

            return EntitySearchResult.NotFound;
        }
    } 
}