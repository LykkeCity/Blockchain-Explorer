using System;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;

namespace Sevices.Bitcoin.Models
{

    public static class DataTimeUtils
    {
        private static DateTime _baseDateTime = new DateTime(1970, 1, 1);

        public static DateTime FromUnixDateTime(this uint unixDateTime)
        {
            return _baseDateTime.AddSeconds(unixDateTime);
        }
    }


    public class GetBlockRpcModel
    {
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("tx")]
        public string[] Tx { get; set; }
        [JsonProperty("time")]
        private uint Time { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("previousblockhash")]
        public string Previousblockhash { get; set; }
        [JsonProperty("nextblockhash")]
        public string Nextblockhash { get; set; }
        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }
        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }
        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        public DateTime GetTime()
        {
            return Time.FromUnixDateTime();
        }

        public bool IsLastBlock()
        {
            return string.IsNullOrEmpty(Nextblockhash);
        }

    }
}