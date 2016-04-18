using System.Threading.Tasks;

namespace Core.BitcoinNinja
{
    public interface IBitcoinNinjaReaderRepository
    {
        Task<ITransactionNinja> GetTransactionAsync(string txId);
        Task<IBlockNinja> GetInformationBlockAsync(string blockHesh);
        Task<IlastBlockNinja> GetLastBlockAsync();
    }
}