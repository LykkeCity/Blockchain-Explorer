using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts
{
    public interface IBlockProvider
    {
        Task<Block> GetBlock(string id);
        Task<Block> GetLastBlock();
    }
}
