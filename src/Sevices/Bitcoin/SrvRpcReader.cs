using System;
using System.Threading.Tasks;
using Core.Bitcoin;
using Sevices.Bitcoin.Models;

namespace Sevices.Bitcoin
{


   

    public class SrvRpcReader
    {
        private readonly BitcoinRpcClient _bitcoinRpc;

        public SrvRpcReader(BitcoinRpcClient bitcoinRpc)
        {
            _bitcoinRpc = bitcoinRpc;
        }

        
    }
}