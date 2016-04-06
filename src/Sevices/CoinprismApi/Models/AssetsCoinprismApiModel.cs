using System.Linq;
using Core.Bitcoin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sevices.CoinprismApi.Models
{
    public class AssetsCoinprismApiModel : IAssets
    {
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }
        [JsonProperty("metadata_url")]
        public string MetadataUrl { get; set; }
        [JsonProperty("final_metadata_url")]
        public string FinalMetadataUrl { get; set; }
        [JsonProperty("verified_issuer")]
        public bool VerifiedIssuer { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("contract_url")]
        public string ContractUrl { get; set; }
        [JsonProperty("name_short")]
        public string NameShort { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("description_mime")]
        public string DescriptionMime { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("divisibility")]
        public int Divisibility { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }





}