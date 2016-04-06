using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureRepositories.Bitcoin;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Sevices.Bitcoin;
using Sevices.BitcoinNinja;
using Sevices.CoinprismApi;

namespace ScriptInvoker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var log = new LogToConsole();
            //var conectionString = "";
            //   var tr = new TransactionRepository(new AzureTableStorage<TransactionEntity>(conectionString, "Transaction", log), new AzureTableStorage<AzureIndex>(conectionString, "TransactionIdx", log));
            //tr.UpdateIndexec().Wait();
            var s = new CoinprismApiSettings
            {
                Network = "test",
                UrlCoinprismApi = "https://api.coinprism.com/v1/",
                UrlCoinprismApiTestnet = "https://testnet.api.coinprism.com/v1/"
            };
            var r = new SrvCoinprismReader(s);

            r.GetAssetOwnersDataAsync("oXjVb4XRo23tp53QFgVUxLhctPKPt6Q127").Wait();

            Console.ReadLine();
        }
    }
}
