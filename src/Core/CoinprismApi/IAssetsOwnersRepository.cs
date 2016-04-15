using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Bitcoin;
using Newtonsoft.Json;

namespace Core.Bitcoin
{
    public interface IAssetsOwners
    {
        long BlockHeight { get; }
        string AssetId { get; }
        string Name { get; }
        IOwner[] Owners { get; }
    }

    public interface IOwner
    {
        string Address { get; set; }
        long AssetQuantity { get; set; }
    }
}

    public class DeserializeAssetsOwner : IAssetsOwners
    {
        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }
        public string Name { get; set; }
        [JsonProperty("owners")]
        public DeserializeOwner[] Owners { get; set; }
        IOwner[] IAssetsOwners.Owners => Owners.Cast<IOwner>().ToArray();
    }
    public class DeserializeOwner : IOwner
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("asset_quantity")]
        public long AssetQuantity { get; set; }
    }


public interface IAssetsOwnersRepository
{
    Task<IAssetsOwners> GetAssetsOwnersDataAsync(string assetId);
    Task WriteAssetsOwnersDataAsync(IAssetsOwners assetOwnersData);
}

