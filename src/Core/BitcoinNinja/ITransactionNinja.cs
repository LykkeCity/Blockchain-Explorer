using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface ITransactionNinja
    {
        string TxId { get; }
        BlockInfoForDetalisTransactionNinja Block { get; }
        DeserializeInputsNinja[] DeserializeInputs { get; }
        DeserializeOutputsNinja[] DeserializeOutputs { get; }
        long Fees { get;  }
        bool IsCoinBase { get;  }
        bool IsColor { get;  }
        IDictionary<string, Asset> Asset { get; }
        long TotalOut { get; }
    }



    public class ListTransaction
    {
        [JsonProperty("hash")]
        public string TxId { get; set; }
        [JsonProperty("isColor")]
        public bool IsColor { get; set; }
    }






}