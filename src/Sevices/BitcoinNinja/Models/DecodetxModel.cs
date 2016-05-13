using System;
using Core.BitcoinNinja;
using NBitcoin;
using Newtonsoft.Json;

namespace Sevices.BitcoinNinja.Models
{



    public class DecodetxModel : IDecodetxNinja
    {
        [JsonProperty("transactionId")]
        public string Hash { get; set; }
        [JsonProperty("isCoinbase")]
        public bool IsCoinbase { get; set; }
        [JsonProperty("firstSeen")]
        public DateTime DateTime { get; set; }
        [JsonProperty("fees")]
        public long Fees { get; set; }
        [JsonProperty("block")]
        public DecodetxBlockModel Bloсk { get; set; }
        [JsonProperty("spentCoins")]
        public DecodetxInputsAndOutputsModel[] Inputs { get; set; }
        [JsonProperty("receivedCoins")]
        public DecodetxInputsAndOutputsModel[] Outputs { get; set; }


    }



}