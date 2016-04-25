using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface IAssetDetailForAddressNinja
    {
        string AssetId { get; }
        long Quantity { get; }
        long Received { get; }
    }

    public class AssetsForAddress : IAssetDetailForAddressNinja
    {
        [JsonProperty("asset")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
        [JsonProperty("received")]
        public long Received { get; set; }
    }

    public interface IAsset
    {
         IEnumerable<DeserializeInputsNinja> AssetDataInput { get; }
         IEnumerable<DeserializeOutputsNinja> AssetDataOutput { get; }
    }

    public class Asset : IAsset
    {
        public IEnumerable<DeserializeInputsNinja> AssetDataInput { get; set; }
        public IEnumerable<DeserializeOutputsNinja> AssetDataOutput { get; set; }
    }


}