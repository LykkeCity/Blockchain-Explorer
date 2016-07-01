using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinRpc.Client
{
    public class BitcoinRpcSettings
    {
        public string User { get; set; }
        public string Password { get; set; }
        public Uri Url { get; set; }
    }

    public class BitcoinRpcClient
    {
        public Uri Url { get; set; }
        private readonly ICredentials _credentials;

        public BitcoinRpcClient(BitcoinRpcSettings settings)
        {
            Url = settings.Url;
            _credentials = new NetworkCredential(settings.User, settings.Password);
        }

        private async Task<string> InvokeMethod(string a_sMethod, params object[] a_params)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
            webRequest.Credentials = _credentials;

            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";

            JObject joe = new JObject();
            joe["jsonrpc"] = "1.0";
            joe["id"] = "1";
            joe["method"] = a_sMethod;

            if (a_params != null)
            {
                if (a_params.Length > 0)
                {
                    JArray props = new JArray();
                    foreach (var p in a_params)
                    {
                        props.Add(p);
                    }
                    joe.Add(new JProperty("params", props));
                }
            }

            string s = JsonConvert.SerializeObject(joe);
            byte[] byteArray = Encoding.UTF8.GetBytes(s);

            using (Stream dataStream = await webRequest.GetRequestStreamAsync())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            {
                using (Stream str = webResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(str))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
            }
        }

        public async Task<GetRawTransactionPrcModel> GetRawTransactionAsync(string txid, int jsonResult = 1)
        {
            var json = await InvokeMethod("getrawtransaction", txid, jsonResult);
            var result = JsonConvert.DeserializeObject<TransportRpcModel<GetRawTransactionPrcModel>>(json);
            return result.Result;
        }

        public async Task<GetBlockRpcModel> GetBlockAsync(string hash)
        {
            var json = await InvokeMethod("getblock", hash);
            var result = JsonConvert.DeserializeObject<TransportRpcModel<GetBlockRpcModel>>(json);
            return result.Result;
        }

        public async Task<int> GetBlockCountAsync()
        {
            var json = await InvokeMethod("getblockcount");
            var result = JsonConvert.DeserializeObject<TransportRpcModel<int>>(json);
            return result.Result;
        }

        public async Task<RpcTransaction> GetTransactionByTxIdAsync(string txId)
        {
            var tx = await GetRawTransactionAsync(txId);

            if (tx == null)
            {
                return null;
            }

            var block = await GetBlockAsync(tx.Blockhash);

            return RpcTransaction.Create(tx, block.Height);
        }
    }

    public class RpcTransaction
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

        public static RpcTransaction Create(GetRawTransactionPrcModel txData, long height)
        {
            return new RpcTransaction 
            {
                Blockhash = txData.Blockhash,
                Txid = txData.Txid,
                Confirmations = txData.Confirmations,
                Time = GetTime(txData.Time),
                Height = height
            };
        }
    }
}