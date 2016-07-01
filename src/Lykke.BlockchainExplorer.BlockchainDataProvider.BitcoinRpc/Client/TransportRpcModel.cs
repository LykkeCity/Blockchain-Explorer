using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinRpc.Client
{
    public class TransportRpcModel<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}