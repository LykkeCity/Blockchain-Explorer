using Lykke.BlockchainExplorer.BitcoinBridge.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BitcoinBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ServiceLocator.ServiceLocator.Initialize(selfInit: true);

                var bt = ServiceLocator.ServiceLocator.Resolve<BlockTransfer>();

                Task.Run(async delegate
                {
                    await bt.Start();
                });

                Console.WriteLine("Started...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
