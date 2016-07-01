using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Repository
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task UpdateAddress(Address address); 
    }
}
