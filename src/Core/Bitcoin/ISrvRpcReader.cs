using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface ISrvRpcReader
    {
        Task<ITransaction> GetTransactionByTxIdAsync(string txid);
    }
}