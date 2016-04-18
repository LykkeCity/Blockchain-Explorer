using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.BitcoinNinja;
using Common;
using Common.Json;
using NBitcoin;
using NBitcoin.BitcoinCore;
using NBitcoin.BouncyCastle.Utilities.Encoders;
using NBitcoin.OpenAsset;
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
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";

            using (var webResponse = await webRequest.GetResponseAsync())
            {
                using (var str = webResponse.GetResponseStream())
                {
                    using (var sr = new StreamReader(str))
                    {
                        return await sr.ReadToEndAsync();
                    }
                }
            }
        }

        public async Task<ITransactionNinja> GetTransactionAsync(string txId)
        {

            var json = await InvokeMethod(Url + "transactions/" + txId);
            var result = JsonConvert.DeserializeObject<TransactionNinjaModel>(json);
            var transactionInfo = Transaction.Parse(result.Hex);
            result.IsCoinBase = transactionInfo.IsCoinBase;
            result.IsColor = transactionInfo.HasValidColoredMarker();
            if (!result.IsColor) return result;
            json = await InvokeMethod(Url + "transactions/" + txId + "?colored=true");
            result = JsonConvert.DeserializeObject<TransactionNinjaModel>(json);
           
            return result;
        }

        public async Task<IlastBlockNinja> GetLastBlockAsync()
        {
            var json = await InvokeMethod(Url + "blocks/tip");
            var result = JsonConvert.DeserializeObject<LastBlockModel>(json);
            return result;
        }

        private async Task<BlockNinjaModel> GetBlockAsync(string block)
        {
            var json = await InvokeMethod(Url + "blocks/" + block);
            var result = JsonConvert.DeserializeObject<BlockNinjaModel>(json);
            return result;
        }

        public async Task<IBlockNinja> GetInformationBlockAsync(string blockHesh)
        {
            var infoBlockApiNinja = await GetBlockAsync(blockHesh);
            var parseBlock = Block.Parse(infoBlockApiNinja.Hex);            
            var trListForBlockJson = GetTransactionsForBlockInString(infoBlockApiNinja.Hex);
            var trListForBlock = JsonConvert.DeserializeObject<TransactionParseModel>(trListForBlockJson);
            return BlockNinjaEntity.Create(infoBlockApiNinja, parseBlock, trListForBlock);
        }


        private static string GetTransactionsForBlockInString(string blockHex)
        {
            var block = Block.Parse(blockHex);
            var jsonBlock = "{\"tr\":[";
            var count = block.Transactions.Count;
            for (var i = 0; i < count; i++)
            {
                jsonBlock += "{";
                jsonBlock += "\"hash\":\"" + block.Transactions[i].GetHash() + "\", \"isColor\":\"" + block.Transactions[i].HasValidColoredMarker() + "\"}";
                if (i < count - 1)
                    jsonBlock += ',';
            }
            jsonBlock += "]}";       
            return jsonBlock;
        }

    }
}