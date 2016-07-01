using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinRpc.Client
{
    public class GetRawTransactionPrcModel
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }
        [JsonProperty("blockhash")]
        public string Blockhash { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }
        [JsonProperty("time")]
        public uint Time { get; set; }
        [JsonProperty("vin")]
        public VinModel[] Vin { get; set; }
        [JsonProperty("vout")]
        public VoutModel[] Vout { get; set; }

        public DateTime GetTime()
        {
            return Time.FromUnixDateTime();
        }

        public class VinModel
        {
            [JsonProperty("txid")]
            public string Txid { get; set; }
            [JsonProperty("vout")]
            public int Vout { get; set; }
        }

        public class VoutModel
        {
            [JsonProperty("value")]
            public double Value { get; set; }
            [JsonProperty("n")]
            public int N { get; set; }
            [JsonProperty("scriptPubKey")]
            public ScriptPubKeyModel ScriptPubKey { get; set; }

            public class ScriptPubKeyModel
            {
                [JsonProperty("addresses")]
                public string[] Addresses { get; set; }
            }
        }
    }
}