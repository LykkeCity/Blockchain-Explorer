using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.NBitcoinIndexer.Model
{
    public class BlockInformation
    {
        public BlockInformation()
        {

        }
        public BlockInformation(BlockHeader header)
        {
            BlockId = header.GetHash();
            BlockHeader = header;
            BlockTime = header.BlockTime;
            Height = -1;
            Confirmations = -1;
        }
        public uint256 BlockId
        {
            get;
            set;
        }

        public BlockHeader BlockHeader
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
        public int Confirmations
        {
            get;
            set;
        }

        public DateTimeOffset MedianTimePast
        {
            get;
            set;
        }

        public DateTimeOffset BlockTime
        {
            get;
            set;
        }
    }

    internal class GetTransactionResponse
    {
        public GetTransactionResponse()
        {
            ReceivedCoins = new List<NBitcoin.ICoin>();
            SpentCoins = new List<NBitcoin.ICoin>();
        }

        public NBitcoin.Transaction Transaction
        {
            get;
            set;
        }

        public NBitcoin.uint256 TransactionId
        {
            get;
            set;
        }

        public bool IsCoinbase
        {
            get;
            set;
        }

        public BlockInformation Block
        {
            get;
            set;
        }

        public List<NBitcoin.ICoin> SpentCoins
        {
            get;
            set;
        }

        public List<NBitcoin.ICoin> ReceivedCoins
        {
            get;
            set;
        }

        public DateTimeOffset FirstSeen
        {
            get;
            set;
        }

        public NBitcoin.Money Fees
        {
            get;
            set;
        }
    }
}
