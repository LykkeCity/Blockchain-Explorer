using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sevices.Bitcoin.Models;

namespace Sevices.Bitcoin
{
    public class SrvBitcoinRpcSettings
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string HostPort { get; set; }

    }

    public class SrvBitcoinRpc
    {
        private readonly SrvBitcoinRpcSettings _settings;

        private readonly ICredentials _credentials;

        public SrvBitcoinRpc(SrvBitcoinRpcSettings settings)
        {
            _settings = settings;
            _settings.HostPort = _settings.HostPort.AddLastSymbolIfNotExists('/');
            _credentials = new NetworkCredential(_settings.User, _settings.Password);
        }


        private async Task<string> InvokeMethod(string a_sMethod, params object[] a_params)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_settings.HostPort);
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
            // serialize json for the request
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

    }
}