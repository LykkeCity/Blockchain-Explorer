using System.IO;
using System.Net;
using System.Threading.Tasks;
using Core.BitcoinNinja;
using Common;
using Newtonsoft.Json;
using Sevices.BitcoinNinja.Models;

namespace Sevices.BitcoinNinja
{
    public class SrvBitcoinNinjaSettings
    {
        public string UrlMainNinja { get; set; }
        public string UrlTestNetNinja { get; set; }
        public string Network { get; set; }

    }

    public class SrvBitcoinNinjaReader: IBitcoinNinjaReaderRepository
    {
        private string Url { get; set; }

        public SrvBitcoinNinjaReader(SrvBitcoinNinjaSettings settings)
        {
            var settingsNinja = settings;
            Url = settingsNinja.Network == "main" ? settingsNinja.UrlMainNinja.AddLastSymbolIfNotExists('/') : settingsNinja.UrlTestNetNinja.AddLastSymbolIfNotExists('/');
        }

        private async Task<string> InvokeMethod(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";

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

        public async Task<ITransactionNinja> GetTransactionAsync(string txId)
        {
            var json = await InvokeMethod(Url+"transactions/"+ txId + "?colored=true");
            var result = JsonConvert.DeserializeObject<TransactionNinjaModel>(json);
            return result;
        }
    }
}