using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureRepositories.Bitcoin;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;

namespace ScriptInvoker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var log = new LogToConsole();
            var conectionString = "";
         //   var tr = new TransactionRepository(new AzureTableStorage<TransactionEntity>(conectionString, "Transaction", log), new AzureTableStorage<AzureIndex>(conectionString, "TransactionIdx", log));
            //tr.UpdateIndexec().Wait();
            Console.ReadLine();
        }
    }
}
