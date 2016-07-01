using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.Coinprism.Client
{
    public class AssetsCoinprismApiModel
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

    public class DeserializeAssetsOwner
    {
        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }
        public string Name { get; set; }
        [JsonProperty("owners")]
        public DeserializeOwner[] Owners { get; set; }
    }
    public class DeserializeOwner
    {
        [JsonProperty("address")] 
        public string Address { get; set; }
        [JsonProperty("asset_quantity")]
        public long AssetQuantity { get; set; }
    }
}