using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinRpc.Client;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinRpc
{
    public class BlockAdapter : IBlockProvider
    {
        private BitcoinRpcClient _client;

        public BlockAdapter()
        {
            var settings = new BitcoinRpcSettings()
            {
                Url = new Uri(""),
                User = "", 
                Password = ""
            };

            _client = new BitcoinRpcClient(settings);
        }

        public async Task<Block> GetBlock(string id)
        {
            var b = await _client.GetBlockAsync(id);

            var block = new Block() 
            {  
                Hash = b.Hash,
                Height = b.Height,
                Time = b.GetTime(),
                Difficulty = b.Difficulty,
                MerkleRoot = b.Merkleroot, 
                Nonce = b.Nonce,
                PreviousBlock = b.Previousblockhash
            }; 

            return block; 
        }

        public Task<Block> GetLastBlock()
        {
            throw new NotImplementedException();
        }
    }
}  
