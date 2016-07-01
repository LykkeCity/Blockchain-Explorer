using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja.Client
{
    internal class BlockNinjaModel
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public DateTime Time => AdditionalInformation.Time;
        public long Confirmations => AdditionalInformation.Confirmations;
        [JsonProperty("block")]
        public string Hex { get; set; }
    }

    internal class TransactionParseModel
    {
        [JsonProperty("tr")]
        public ListTransaction[] ListTranasction { get; set; }
    }

    internal class AdditionalInformation
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

    internal class LastBlockModel
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public string Hash => AdditionalInformation.Hash;
    }


    internal class BlockNinjaEntity
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
        public ListTransaction[] Transactions { get; set; }
        
        public static BlockNinjaEntity Create(BlockNinjaModel blockInfo, Block block, ListTransaction[] listTransactions)
        {
            return new BlockNinjaEntity
            {
                Confirmations = blockInfo.Confirmations,
                Time = blockInfo.Time,
                Height = blockInfo.Height,
                Hash = block.Header.ToString(),
                TotalTransactions = block.Transactions.Count,
                Transactions = listTransactions,
                Difficulty = block.Header.Bits.Difficulty,
                MerkleRoot = block.Header.HashMerkleRoot.ToString(),
                PreviousBlock = block.Header.HashPrevBlock.ToString(),
                Nonce = block.Header.Nonce
            };
        }
    }

    internal class ListTransaction
    {
        [JsonProperty("hash")]
        public string TxId { get; set; }
        [JsonProperty("isColor")]
        public bool IsColor { get; set; }
        public Uri MetadataUrl { get; set; }
    }
}
