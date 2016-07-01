using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts.Services
{
    public interface IBlockService
    {
        IBlockchainDataProvider BlockchainDataProvider { get; set; }
        IBlockRepository BlockRepository { get; set; }

        Task<Block> GetBlock(string id);
        Task<Block> GetLastBlock(); 
    }
}
