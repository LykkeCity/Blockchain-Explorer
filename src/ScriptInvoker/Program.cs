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
            var s = new SrvBitcoinNinjaSettings
            {
                Network = "test",
                UrlMainNinja = "http://btc-ninja.azurewebsites.net/",
                UrlTestNetNinja = "https://testnet-ninja.azurewebsites.net/"
            };
            var r = new SrvBitcoinNinjaReader(s);

            string blockHex = "04000000f841b61270210221f069880ae44a3d2edf61a0f9d2860a1aadf0040000000000ac6889169a8eb331592b243034bb13ad0bd431d4b9f1f87f6fd6f54049329c6f4f3df556400f081bd1b028d60401000000010000000000000000000000000000000000000000000000000000000000000000ffffffff040367500bffffffff04897064230000000017a914349ef962198fcc875f45e786598272ecace9818d8781dddc010000000017a914349ef962198fcc875f45e786598272ecace9818d870000000000000000226a200000000000000000000000000000000000000000000000000000ffff0000000000000000000000000a6a0897fb999e51000000000000000100000001d1655a4fe5fd888475d29616fd9d527de8b96b58dbad9a43f522181004eda44f000000006a47304402205806c4eb29c0b6d047ea082918bd92930338c24d02061b9913dd5efbb6cfa50202206ba6b29d4acc3fcd3b17943fdf279b721135059208f6a8167461cb49cb4ed7e4012103d76ff5056e04401cbd2b435ca9d1db340221b79eb9fc63a658a2f05972511bd2ffffffff02d7752309000000001976a9141caff9df971f7b07e1b1e5a05003dfdbce5df35d88acc8297016000000001976a9144edf7dea2e375e89eb55ca1bda8f04f59b28e3ba88ac000000000100000001e03f3da2f6f57f91755744cba4d1cedba4c527fdb612c87c54b27a1859a6ee9d010000006a47304402204b438ca59592cb4091b1085d01cb9e4c1108589f40fda9768e000cfa5d48621c0220210b9a96e7c530ac46667f9332c24b13fed7a76a2b59930d26041217cf6b75790121024c4f2c975f1a4d728b8e36591e7e1ef20b32763f74bddb827470408c9914df64ffffffff02a6dce90a000000001976a9140fb4e944de36fda4b8f59492e022db49781c6a5388ac341d860b000000001976a914a61c7f59556ae3c27d3da80b5f4caf442ca5c65e88ac000000000100000001fe1ef1ca1221fe1d9d23cc9ddf918afa56f7e05a8f13a603abc8d9d423a1ac5b010000006a4730440220586b47db29f6df40cc0d692252cd2bcb9d8cfa20af3d6eeea9f89424b9c042c702202182df5e4b0a376a0fcaddf69a3b51e95a649a480c80709e684a861f3348a41c012102f4e38e71af2b8af80242407cb9cc3a063d5ee0dc4232156a950df4c81e05a6d5ffffffff0146ed850b000000001976a914f63ab04c5bda80aa13d729e1a60acf2b67d01d9688ac00000000";

            var t = r.GetTrGetInformationBlockAsync("000000000091bc0e60223a22667a1f0e79a93e2fa4ebdda15cad49cd7334d533").Result;

            Console.ReadLine();
        }
    }
}
