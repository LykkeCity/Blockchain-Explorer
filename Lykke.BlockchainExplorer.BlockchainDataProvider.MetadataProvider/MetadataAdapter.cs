using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.MetadataProvider
{
    public class MetadataAdapter : IMetadataProvider
    {
        public async Task<Asset> GetMetadataContent(Uri url)
        {
            var result = await InvokeMethod(url);
            var assetData = JsonConvert.DeserializeObject<AssetData>(result);

            var asset = new Asset()
            {
                Id = assetData.AssetIds.First(),
                ContractUrl = assetData.ContractUrl,
                NameShort = assetData.NameShort,
                Name = assetData.Name,
                Issuer = assetData.Issuer,
                Description = assetData.Description,
                DescriptionMime = assetData.DescriptionMime,
                Type = assetData.Type,
                Divisibility = assetData.Divisibility,
                IconUrl = assetData.IconUrl,
                ImageUrl = assetData.ImageUrl,
                MetadataUrl = url.ToString(),
                FinalMetadataUrl = url.ToString(),
                Version = assetData.Version
            };
             
            return asset;
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
    }
}
