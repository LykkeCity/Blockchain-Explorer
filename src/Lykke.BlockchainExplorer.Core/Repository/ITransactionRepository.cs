using Lykke.BlockchainExplorer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task SaveAsImport(Transaction entity);
        Task<bool> IsImported(string id);
        Task SetAsImported(string id);
    }
}