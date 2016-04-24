using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface IAssetDetailsNinja
    {
        string AssetId { get; }
        long Quantity { get; }
        string Address { get; }
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