using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Domain
{
    public class Asset
    {
        public string Id { get; set; }
        public string MetadataUrl { get; set; }
        public string FinalMetadataUrl { get; set; }
        public bool VerifiedIssuer { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string ContractUrl { get; set; }
        public string Issuer { get; set; }
        public string Description { get; set; }
        public string DescriptionMime { get; set; }
        public string Type { get; set; }
        public int Divisibility { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Version { get; set; }
        public long Quantity { get; set; }
        public long Received { get; set; }
        public IList<In> Input { get; set; }
        public IList<Out> Output { get; set; }
    }
}
