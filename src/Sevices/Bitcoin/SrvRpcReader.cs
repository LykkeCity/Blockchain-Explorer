using System;
using System.Threading.Tasks;
using Core.Bitcoin;
using Sevices.Bitcoin.Models;

namespace Sevices.Bitcoin
{


    public class SrvRpcReaderEntyti: ITransaction
    {
        public string Txid { get; set; }
        public string Blockhash { get; set; }
        public long Confirmations { get; set; }
        public DateTime Time { get; set; }
        public long Height { get; set; }


        public static DateTime GetTime(uint time)
        {
            return time.FromUnixDateTime();
        }

        public static SrvRpcReaderEntyti Create(GetRawTransactionPrcModel txData, long height)
        {
           return new SrvRpcReaderEntyti
           {
               Blockhash = txData.Blockhash,
               Txid = txData.Txid,
               Confirmations = txData.Confirmations,
               Time = GetTime(txData.Time),
               Height = height
           };
        }

    }


    public class SrvRpcReader: ISrvRpcReader
    {
        private readonly SrvBitcoinRpc _bitcoinRpc;

        public SrvRpcReader(SrvBitcoinRpc bitcoinRpc)
        {
            _bitcoinRpc = bitcoinRpc;
        }

        public async Task<ITransaction> GetTransactionByTxIdAsync(string txid)
        {
            var tx = await _bitcoinRpc.GetRawTransactionAsync(txid);
            if (tx == null)
                return null;

            var block = await _bitcoinRpc.GetBlockAsync(tx.Blockhash);

            return SrvRpcReaderEntyti.Create(tx, block.Height);

        }
    }
}