using Core.BitcoinNinja;
using Newtonsoft.Json;

namespace Sevices.BitcoinNinja.Models
{
    public class AddressNinjaModel : IAddressNinja
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

    public class AddressSummaryConfirmed
    {
        [JsonProperty("confirmed")]
        public AddressSummary Confirmed { get; set; }
    }

    public class AddressSummary
    {
        [JsonProperty("transactionCount")]
        public int TotalTransactions { get; set; }
        [JsonProperty("amount")]
        public long Balance { get; set; }
        [JsonProperty("assets")]
        public AssetsForAddress[] Assets { get; set; }
    }


    public class AddressListTransaction
    {
        [JsonProperty("operations")]
        public AddressListTr[] Operations { get; set; }
    }

    public class AddressListTr
    {
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
    }

}