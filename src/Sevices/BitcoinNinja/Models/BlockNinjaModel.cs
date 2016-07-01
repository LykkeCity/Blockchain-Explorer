using System;
using Core.BitcoinNinja;
using NBitcoin;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sevices.BitcoinNinja.Models
{
    public class BlockNinjaModel 
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public DateTime Time => AdditionalInformation.Time;
        public long Confirmations => AdditionalInformation.Confirmations;
        [JsonProperty("block")]
        public string Hex { get; set; }
    }

    public class TransactionParseModel
    {
        [JsonProperty("tr")]
        public ListTransaction[] ListTranasction { get; set; }
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

    public class LastBlockModel : ILastBlockNinja
    {
        [JsonProperty("additionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }
        public long Height => AdditionalInformation.Height;
        public string Hash => AdditionalInformation.Hash;
    }


    public class BlockNinjaEntity : IBlockNinja
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
        public ListTransaction[] ListTranasction { get; set; }

        public static BlockNinjaEntity Create(BlockNinjaModel infoBlockApiNinja, Block parseBlock, ListTransaction[] transactionList)
        {
            return new BlockNinjaEntity
            {
                Confirmations = infoBlockApiNinja.Confirmations,
                Time = infoBlockApiNinja.Time,
                Height = infoBlockApiNinja.Height,
                Hash = parseBlock.Header.ToString(),
                TotalTransactions = parseBlock.Transactions.Count,
                ListTranasction = transactionList,
                Difficulty = parseBlock.Header.Bits.Difficulty,
                MerkleRoot = parseBlock.Header.HashMerkleRoot.ToString(),
                PreviousBlock = parseBlock.Header.HashPrevBlock.ToString(),
                Nonce = parseBlock.Header.Nonce
            };
        }
    }





}