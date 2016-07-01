using AzureRepositories.Bitcoin;
using AzureRepositories.BitcoinNinja;
using AzureStorage.Blobs;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Core.Bitcoin;
using Core.BitcoinNinja;
using Microsoft.Extensions.DependencyInjection;

namespace AzureRepositories
{
    public static class IocBinder
    {
        public static void BindAzureRepositories(this IServiceCollection services, string connectionString, ILog log)
        {
            services.AddInstance<ILastImportendBlockHash>(new LastImportendBlockHash(new AzureTableStorage<LastImportedBlockHashEntity>(connectionString, "Settings", log)));
            services.AddInstance<IBitcoinBlockRepository>(new BitcoinBlockRepository(new AzureTableStorage<BicoinBlockEntity>(connectionString, "Blocks", log)));
            services.AddInstance<ITransactionRepository>(new TransactionRepository(new AzureTableStorage<TransactionEntity>(connectionString, "Transaction", log)));
            services.AddInstance<IOutputsRepository>(new OutputsRepository(new AzureTableStorage<OutputEntity>(connectionString, "Outputs", log)));
            services.AddInstance<IInputsRepository>(new InputsRepository(new AzureTableStorage<InputEntity>(connectionString, "Inputs", log)));
            services.AddInstance<IAssetsRepository>(new AssetsRepository(new AzureTableStorage<AssetsEntity>(connectionString, "Assets", log)));
            services.AddInstance<IAssetsOwnersRepository>(new AssetsOwnersRepository(new AzureTableStorage<AssetsOwnersEntity>(connectionString, "AssetsOwners", log), new AzureBlobStorage(connectionString)));
            services.AddInstance<IBlockNinjaRepository>(new BlockRepository(new AzureTableStorage<BlockNinjaEntity>(connectionString, "BlocksNinja", log), new AzureBlobStorage(connectionString)));
        }
    }
}