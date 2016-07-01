using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureRepositories;
using AzureRepositories.Log;
using AzureStorage.Tables;
using Common.Log;
using Microsoft.Extensions.DependencyInjection;
using Sevices.Bitcoin;


namespace BitcoinBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var appSettings = AppSettingsReader.ReadSettings(); 

                IServiceCollection services = new ServiceCollection();
                 
                var log = new LogToTable(new AzureTableStorage<LogEntity>(appSettings.ConnectionString, "Log", null));

                services.AddInstance<ILog>(log);
                services.BindAzureRepositories(appSettings.ConnectionString, log);
                services.AddInstance(new BitcoinRpcClient(appSettings.BitcoinRpcSettings));

                var serviceProvider = services.BuildServiceProvider();
                var start = ActivatorUtilities.CreateInstance<JobsBlockTransfer>(serviceProvider);
                //var start = ioc.CreateInstance<JobsBlockTransfer>();
                start.Start(appSettings.FirstHashBlock);
                //Console.WriteLine("Started...");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
        }
    }
}
