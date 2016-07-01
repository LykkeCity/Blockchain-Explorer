using Lykke.BlockchainExplorer.Core.Enums;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts.Services
{
    public interface ISearchService
    {
        Task<EntitySearchResult> SearchEntityById(string id);
    }
}