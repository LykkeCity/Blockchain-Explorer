using System;
using System.Threading.Tasks;
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

    public class BlockInfoForDetalisTransactionNinja
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

    public class DeserializeBlockNinja : IBlockNinja
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

    public interface IBlockNinjaRepository
    {
        Task<IBlockNinja> GetBlockDataAsync(string blockId);
        Task WriteBlockDataAsync(IBlockNinja blockData);
    }




}