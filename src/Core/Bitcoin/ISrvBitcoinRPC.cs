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
        string[] Tx { get; }
        long Height { get; }
        string Previousblockhash { get; }
        string Nextblockhash { get; }
        double Difficulty { get; }
        string Merkleroot { get; }
        long Nonce { get; }
    }

    public interface IBitcoinTx
    {

    }

   
}
