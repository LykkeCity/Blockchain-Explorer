using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface IInputsNinja
    {
        string Address { get; }
        string TxId { get; }
        int Index { get; }
        long Value { get; }
        string AssetId { get; }
        long Quantity { get; }
    }

    public class DeserializeInputsNinja : IInputsNinja
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