using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface ITransactionNinja
    {
        string TxId { get; }
        BlockNinja Block { get; }
        InputsNinja[] Inputs { get; }
        OutputsNinja[] Outputs { get; }
        long Fees { get;  }
        IEnumerable<InputsNinja> AssetData { get; }
    }

    public class BlockNinja : IBlockNinja
    {
        [JsonProperty("blockId")]
        public string Hash { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("blockTime")]
        public DateTime Time { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }
    }

    public class InputsNinja : IInputsNinja
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }

    public class OutputsNinja : IOutputsNinja
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }



}