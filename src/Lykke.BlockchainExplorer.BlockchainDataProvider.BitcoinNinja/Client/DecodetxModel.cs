using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja.Client
{
    internal class DecodetxModel
    {
        [JsonProperty("transactionId")]
        public string Hash { get; set; }
        [JsonProperty("isCoinbase")]
        public bool IsCoinbase { get; set; }
        [JsonProperty("firstSeen")]
        public DateTime DateTime { get; set; }
        [JsonProperty("fees")]
        public long Fees { get; set; }
        [JsonProperty("block")]
        public DecodetxBlockModel Bloсk { get; set; }
        [JsonProperty("spentCoins")] 
        public DecodetxInputsAndOutputsModel[] Inputs { get; set; }
        [JsonProperty("receivedCoins")]
        public DecodetxInputsAndOutputsModel[] Outputs { get; set; }
    }

    internal class DecodetxBlockModel
    {
        [JsonProperty("blockId")]
        public string Hash { get; set; }
        [JsonProperty("blockHeader")]
        public string Header { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; } 
        [JsonProperty("blockTime")]
        public DateTime DateTime { get; set; }
    }

    internal class DecodetxInputsAndOutputsModel
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("transactionId")]
        public string TransactionHash { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }
        [JsonProperty("redeemScript")]
        public string RedeemScript { get; set; }
    }
}
