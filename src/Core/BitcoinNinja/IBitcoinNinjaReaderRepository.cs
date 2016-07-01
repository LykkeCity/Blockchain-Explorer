using System.Threading.Tasks;

namespace Core.BitcoinNinja
{
    public interface IBitcoinNinjaClient
    {
        Task<ITransactionNinja> GetTransactionAsync(string txId, bool colored = false);
        Task<IBlockNinja> GetInformationBlockAsync(string blockHesh);
        Task<ILastBlockNinja> GetLastBlockAsync();
        Task<IAddressNinja> GetAddressAsync(string address);
        Task<IDecodetxNinja> DecodeTransactionAsync(string txHex);
    }
}