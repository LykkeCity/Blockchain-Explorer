using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lykke.BlockchainExplorer.UI.Models
{
    public class AddressModel
    {
        public Address Address { get; set; }
        public int Count { get; set; }
        public int Start { get; set; }
        public int CurrentPage { get; set; }
        public long Max { get; set; }
    }
}