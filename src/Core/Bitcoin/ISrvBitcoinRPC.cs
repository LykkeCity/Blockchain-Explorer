using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface IBitcoinBlockRPC
    {
        string Hash { get; }
        DateTime Time { get; }
    }

    public interface IBitcoinTx
    {

    }

    public interface ISrvBitcoinRpc
    {
        Task<IBitcoinBlockRPC> GetBitcoinBlockAsync(string hash);
    }
}
