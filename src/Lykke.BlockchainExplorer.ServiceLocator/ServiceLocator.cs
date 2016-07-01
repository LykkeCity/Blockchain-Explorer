using Lykke.BlockchainExplorer.BlockchainDataProvider;
using Lykke.BlockchainExplorer.Core.Contracts;
using Ninject.Modules;
using Ninject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Repository;
using Lykke.BlockchainExplorer.Repository;
using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Log;

namespace Lykke.BlockchainExplorer.ServiceLocator
{
    public static class ServiceLocator 
    {
        private static NinjectModule _serviceModule;
        public static NinjectModule ServiceModule
        {
            get
            {
                if(_serviceModule == null)
                {
                    _serviceModule = new ServiceModule();
                }
                 
                return _serviceModule;
            }
        }

        public static void Initialize(bool selfInit = false)
        {
            if (selfInit)
            {
                SelfInit();

                return;
            } 

            ServiceModule.Load();
        }       

        private static void SelfInit()
        {
            _serviceModule = new TransferModule();

            var modules = new INinjectModule[]
            {
                ServiceModule
            }; 

            var kernel = new StandardKernel(modules);
        }

        public static T Resolve<T>()
        {
            return ServiceModule.Kernel.Get<T>();
        }
    }

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBlockchainDataProvider>().To<BlockchainProvider>();
            this.Bind<IBlockProvider>().To<BlockchainDataProvider.BitcoinNinja.BlockAdapter>();
            this.Bind<ITransactionProvider>().To<BlockchainDataProvider.BitcoinNinja.TransactionAdapter>();
            this.Bind<IAssetProvider>().To<BlockchainDataProvider.Coinprism.AssetAdapter>();
            this.Bind<IAddressProvider>().To<BlockchainDataProvider.BitcoinNinja.AddressAdapter>();
            this.Bind<IMetadataProvider>().To<BlockchainDataProvider.MetadataProvider.MetadataAdapter>();

            this.Bind<IBlockRepository>().To<Repository.SqlServer.BlockRepository>();
            this.Bind<ITransactionRepository>().To<Repository.SqlServer.TransactionRepository>();
            this.Bind<IAssetRepository>().To<Repository.SqlServer.AssetRepository>();
            this.Bind<IAddressRepository>().To<Repository.SqlServer.AddressRepository>();
            this.Bind<ILog>().To<Repository.SqlServer.LogRepository>();

            this.Bind<IBlockService>().To<Services.BlockService>();
            this.Bind<ITransactionService>().To<Services.TransactionService>();
            this.Bind<IAddressService>().To<Services.AddressService>();
            this.Bind<IAssetService>().To<Services.AssetService>();
            this.Bind<ISearchService>().To<Services.SearchService>();
        }
    }

    public class TransferModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBlockchainDataProvider>().To<BlockchainProvider>();
            this.Bind<IBlockProvider>().To<BlockchainDataProvider.NBitcoinIndexer.BlockAdapter>();
            this.Bind<ITransactionProvider>().To<BlockchainDataProvider.BitcoinNinja.TransactionAdapter>();
            this.Bind<IAssetProvider>().To<BlockchainDataProvider.Coinprism.AssetAdapter>();
            this.Bind<IAddressProvider>().To<BlockchainDataProvider.BitcoinNinja.AddressAdapter>();
            this.Bind<IMetadataProvider>().To<BlockchainDataProvider.MetadataProvider.MetadataAdapter>();

            this.Bind<IBlockRepository>().To<Repository.SqlServer.BlockRepository>();
            this.Bind<ITransactionRepository>().To<Repository.SqlServer.TransactionRepository>();
            this.Bind<IAssetRepository>().To<Repository.SqlServer.AssetRepository>();
            this.Bind<IAddressRepository>().To<Repository.SqlServer.AddressRepository>();
            this.Bind<ILog>().To<Repository.SqlServer.LogRepository>();

            this.Bind<IBlockService>().To<Services.BlockService>();
            this.Bind<ITransactionService>().To<Services.TransactionService>();
            this.Bind<IAddressService>().To<Services.AddressService>();
            this.Bind<IAssetService>().To<Services.AssetService>();
            this.Bind<ISearchService>().To<Services.SearchService>();
        }
    }
}