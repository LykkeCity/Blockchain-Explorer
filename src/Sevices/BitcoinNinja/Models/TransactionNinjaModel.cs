using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.BitcoinNinja;
using Newtonsoft.Json;


namespace Sevices.BitcoinNinja.Models
{
    public class TransactionNinjaModel: ITransactionNinja
    {
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("block")]
        public BlockNinja Block { get; set; }
        [JsonProperty("spentCoins")]
        public InputsNinja[] Inputs { get; set; }
        [JsonProperty("receivedCoins")]
        public OutputsNinja[] Outputs { get; set; }
        [JsonProperty("transaction")]
        public string Hex { get; set; }
        [JsonProperty("fees")]
        public long Fees { get; set; }

        public bool IsCoinBase { get; set; }
        public bool IsColor { get; set; }

        public IEnumerable<InputsNinja> AssetData
        {
            get { return Inputs.Where(itm => itm.AssetId != null); }
        }
    }

}
