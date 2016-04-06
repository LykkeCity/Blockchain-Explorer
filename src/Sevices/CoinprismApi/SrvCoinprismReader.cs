using System.IO;
using System.Net;
using System.Threading.Tasks;
using Core.Bitcoin;
using Core.CoinprismApi;
using Common;
using Newtonsoft.Json;
using Sevices.CoinprismApi.Models;

namespace Sevices.CoinprismApi
{

    public class CoinprismApiSettings
    {
        public string UrlCoinprismApi { get; set; }
        public string UrlCoinprismApiTestnet { get; set; }
        public string Network { get; set; }

    }

    public class SrvCoinprismReader : ISrvCoinprismReader
    {

        private string Url { get; set; }

        public SrvCoinprismReader(CoinprismApiSettings settings)
        {
            var coinprismApiSettings = settings;
            Url = coinprismApiSettings.Network == "main" ? coinprismApiSettings.UrlCoinprismApi.AddLastSymbolIfNotExists('/') : coinprismApiSettings.UrlCoinprismApiTestnet.AddLastSymbolIfNotExists('/');
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


        public async Task<IAssets> GetAssetDataAsync(string assetId)
        {
            var json = await InvokeMethod(Url + "assets/" + assetId);
            var result = JsonConvert.DeserializeObject<AssetsCoinprismApiModel>(json);
            return result;
        }

        public async Task<IAssetsOwners> GetAssetOwnersDataAsync(string assetId)
        {
            var json = await InvokeMethod(Url + "assets/" + assetId + "/owners");
            var result = JsonConvert.DeserializeObject<DeserializeAssetsOwner>(json);
            return result;
        }
    }
}