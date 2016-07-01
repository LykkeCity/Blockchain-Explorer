using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Contracts.Services
{
    public interface ITransactionService
    {
        IBlockchainDataProvider BlockchainDataProvider { get; set; }
        ITransactionRepository TransactionRepository { get; set; }
        IAssetRepository AssetRepository { get; set; }
        IBlockService BlockService { get; set; }
        ILog Log { get; set; }

        Task<Transaction> GetTransaction(string id);
    }
}
