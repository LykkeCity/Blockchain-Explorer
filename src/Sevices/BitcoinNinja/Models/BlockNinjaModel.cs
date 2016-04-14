using System;
using Core.BitcoinNinja;
using Newtonsoft.Json;

namespace Sevices.BitcoinNinja.Models
{
    public class BlockNinjaModel 
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public DateTime Time => AdditionalInformation.Time;
        public long Confirmations => AdditionalInformation.Confirmations;
        [JsonProperty("tr")]
        public ListTranasction[] ListTranasction { get; set; }
        [JsonProperty("block")]
        public string Hex { get; set; }
    }

    public class AdditionalInformation
    {
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("blockTime")]
        public DateTime Time { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }
        [JsonProperty("blockId")]
        public string Hash { get; set; }
    }

    public class LastBlockModel : IlastBlockNinja
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public string Hash => AdditionalInformation.Hash;
    }


    public class BlockViewNinjaModel : IBlockNinja
    {
        public string Hash { get; set; }
        public long Height { get; set; }
        public DateTime Time { get; set; }
        public long Confirmations { get; set; }
        public double Difficulty { get; set; }
        public string MerkleRoot { get; set; }
        public long Nonce { get; set; }
        public int TotalTransactions { get; set; }
        public string PreviousBlock { get; set; }
        public ListTranasction[] ListTranasction { get; set; }
    }





}