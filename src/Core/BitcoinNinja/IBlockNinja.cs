using System;
using Newtonsoft.Json;

namespace Core.BitcoinNinja
{
    public interface IBlockNinja
    {
        string Hash { get; }
        long Height { get; }
        DateTime Time { get; }
        long Confirmations { get; }
        double Difficulty { get; }
        string MerkleRoot { get; }
        long Nonce { get; }
        int TotalTransactions { get; }
        string PreviousBlock { get; }
        ListTranasction[] ListTranasction { get;}
    }

    public interface IlastBlockNinja
    {
        long Height { get; }
        string Hash { get; }
    }

    public class ListTranasction
    {
        [JsonProperty("hash")]
        public string TxId { get; set; }
        [JsonProperty("isColor")]
        public bool IsColor { get; set; }
    }





}