using AzureRepositories.Bitcoin;
using AzureStorage.Tables;
using Common.Log;
using Core.Bitcoin;
using Microsoft.Extensions.DependencyInjection;

namespace AzureRepositories
{
    public static class IocBinder
    {
       
        public static void BindAzureRepositories(this IServiceCollection services, string connectionString, ILog log)
        {
            services.AddInstance<ILastImportendBlockHash>(new LastImportendBlockHash(new AzureTableStorage<LastImportendBlockHashEntyti>(connectionString, "Settings", log)));
            services.AddInstance<IBitcoinBlockRepository>(new BitcoinBlockRepository(new AzureTableStorage<BicoinBlockEntity>(connectionString, "Blocks", log)));
            services.AddInstance<ITransactionRepository>(new TransactionRepository(new AzureTableStorage<TransactionEntity>(connectionString, "Transaction", log)));
            services.AddInstance<IOutputsRepository>(new OutputsRepository(new AzureTableStorage<OutputEntity>(connectionString, "Outputs", log)));
            services.AddInstance<IInputsRepository>(new InputsRepository(new AzureTableStorage<InputEntity>(connectionString, "Inputs", log)));
        }
    }
}
