using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja.Client
{
    internal class AddressNinjaModel
    {
        public string Address { get; set; }
        [JsonProperty("coloredAddress")]
        public string ColoredAddress { get; set; }
        [JsonProperty("uncoloredAddress")]
        public string UncoloredAddress { get; set; }
        public long Balance { get; set; }
        public int TotalTransactions { get; set; }
        public AssetsForAddress[] Assets { get; set; }
        public string[] ListTranasctions { get; set; }

    }

    internal class AddressSummaryConfirmed
    {
        [JsonProperty("confirmed")]
        public AddressSummary Confirmed { get; set; }
    }

    internal class AddressSummary 
    {
        [JsonProperty("transactionCount")]
        public int TotalTransactions { get; set; }
        [JsonProperty("amount")]
        public long Balance { get; set; }
        [JsonProperty("assets")]
        public AssetsForAddress[] Assets { get; set; }
    }


    internal class AddressListTransaction
    {
        [JsonProperty("operations")]
        public AddressListTr[] Operations { get; set; }
    }

    internal class AddressListTr
    {
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
    }

    internal class AssetsForAddress
    {
        [JsonProperty("asset")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
        [JsonProperty("received")]
        public long Received { get; set; }
    }
}
