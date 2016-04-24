using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface ITransactionNinja
    {
        string TxId { get; }
        BlockNinja Block { get; }
        DeserializeInputsNinja[] DeserializeInputs { get; }
        DeserializeOutputsNinja[] DeserializeOutputs { get; }
        long Fees { get;  }
        bool IsCoinBase { get;  }
        bool IsColor { get;  }
        IDictionary<string, Asset> Asset { get; } 
    }



    public class BlockNinja
    {
        [JsonProperty("blockId")]
        public string Hash { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("blockTime")]
        public DateTime Time { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }
    }

  




}