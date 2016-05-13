using System;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface IDecodetxNinja
    {
        string Hash { get; set; }
        bool IsCoinbase { get; set; }
        DateTime DateTime { get; set; }
        long Fees { get; set; }
        DecodetxBlockModel Bloсk { get; set; }
        DecodetxInputsAndOutputsModel[] Inputs { get; set; }
        DecodetxInputsAndOutputsModel[] Outputs { get; set; }
    }


    public class DecodetxBlockModel
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

    public class DecodetxInputsAndOutputsModel
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