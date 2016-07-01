using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts
{
    public interface IMetadataProvider
    {
        Task<Asset> GetMetadataContent(Uri url);
    }
}
