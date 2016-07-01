using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.MetadataProvider
{
    internal class AssetData 
    {
        [JsonProperty("asset_ids")]
        public string[] AssetIds { get; set; }
        [JsonProperty("contract_url")]
        public string ContractUrl { get; set; }
        [JsonProperty("name_short")]
        public string NameShort { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
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
        [JsonProperty("link_to_website")]
        public string LinkToWebsite { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}