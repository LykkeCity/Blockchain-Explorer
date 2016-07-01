using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts.Services
{
    public interface IAddressService
    {
        IBlockchainDataProvider BlockchainDataProvider { get; set; }
        IBlockRepository BlockRepository { get; set; } 

        Task<Address> GetAddress(string id);
    }
}
