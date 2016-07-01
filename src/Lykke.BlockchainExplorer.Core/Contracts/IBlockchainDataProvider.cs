using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts
{
    public interface IBlockchainDataProvider : ITransactionProvider, IBlockProvider, IAddressProvider, IAssetProvider, IMetadataProvider
    {
    }
}
