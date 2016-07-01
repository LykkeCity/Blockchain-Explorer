using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider
{
    public class BlockchainProvider : IBlockchainDataProvider
    {
        public ITransactionProvider TransactionProvider { get; set; }
        public IBlockProvider BlockProvider { get; set; }
        public IAddressProvider AddressProvider { get; set; }
        public IAssetProvider AssetProvider { get; set; }
        public IMetadataProvider MetadataProvider { get; set; }
        public ILog Log { get; set; }

        public BlockchainProvider(ITransactionProvider transactionProvider,
                                  IBlockProvider blockProvider,
                                  IAddressProvider addressProvider,
                                  IAssetProvider assetProvider,
                                  IMetadataProvider metadataProvider,
                                  ILog log)
        {
            TransactionProvider = transactionProvider;
            BlockProvider = blockProvider;
            AddressProvider = addressProvider;
            AssetProvider = assetProvider;
            MetadataProvider = metadataProvider;
            Log = log;
        }

        public async Task<Block> GetBlock(string id)
        {
            return await BlockProvider.GetBlock(id); 
        }

        public async Task<Transaction> GetTransaction(string id)
        {
            return await TransactionProvider.GetTransaction(id);
        }

        public async Task<Block> GetLastBlock()
        {
            return await BlockProvider.GetLastBlock();
        }

        public async Task<Address> GetAddress(string id)
        {
            return await AddressProvider.GetAddress(id);
        }

        public async Task<Asset> GetAsset(string id)
        {
            return await AssetProvider.GetAsset(id);
        }

        public async Task<AssetOwners> GetAssetOwners(string id)
        {
            return await AssetProvider.GetAssetOwners(id); 
        }

        public async Task<Asset> GetMetadataContent(Uri url)
        {
            return await MetadataProvider.GetMetadataContent(url);
        }
    }
}