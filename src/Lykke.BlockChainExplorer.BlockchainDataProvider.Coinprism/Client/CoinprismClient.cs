using Lykke.BlockchainExplorer.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.Coinprism.Client
{
    public class CoinprismApiSettings
    {
        public Uri UrlMain { get; set; }
        public Uri UrlTest { get; set; }
        public Network Network { get; set; }
    }

    public class CoinprismClient
    {
        public Uri Url { get; set; }

        public CoinprismClient(CoinprismApiSettings settings)
        {
            Url = getNetworkUri(settings);
        }

        private Uri getNetworkUri(CoinprismApiSettings settings)
        {
            if (settings.Network == Network.Test)
            {
                return settings.UrlTest;
            }
            else
            {
                return settings.UrlMain;
            }
        } 

        private async Task<string> InvokeMethod(Uri url)
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

        public async Task<AssetsCoinprismApiModel> GetAssetDataAsync(string assetId)
        {
            var relativePath = String.Format("assets/{0}", assetId);
            var assetUrl = new Uri(Url, relativePath);

            var json = await InvokeMethod(assetUrl);
            var result = JsonConvert.DeserializeObject<AssetsCoinprismApiModel>(json);

            return result;
        }
        
        public async Task<DeserializeAssetsOwner> GetAssetOwnersDataAsync(string assetId)
        {
            var relativePath = String.Format("assets/{0}/owners", assetId); 
            var assetOwnerUrl = new Uri(Url, relativePath);

            var json = await InvokeMethod(assetOwnerUrl);
            var result = JsonConvert.DeserializeObject<DeserializeAssetsOwner>(json);

            return result;
        }
    }
}
