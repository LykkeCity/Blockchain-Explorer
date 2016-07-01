using System;
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
    public class BitcoinNinjaSettings
    {
        public Uri UrlMain { get; set; }
        public Uri UrlTest { get; set; }
        public Core.Enums.Network Network { get; set; }
    }

    public class BitcoinNinjaClient: IBitcoinNinjaClient
    {
        public Uri Url { get; set; }

        public BitcoinNinjaClient(BitcoinNinjaSettings settings)
        {
            Url = getNetworkUri(settings);
        }

        private Uri getNetworkUri(BitcoinNinjaSettings settings)
        {
            if(settings.Network == Core.Enums.Network.Test)
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

        public async Task<ITransactionNinja> GetTransactionAsync(string txId, bool colored = false)
        {
            var relativePath = String.Format("transactions/{0}", txId);
            var transactionUrl = new Uri(Url, relativePath);

            if(colored)
            {
                var builder = new UriBuilder(transactionUrl);
                builder.Query = "colored=true";
                transactionUrl = builder.Uri;
            }

            var json = await InvokeMethod(transactionUrl);
            var result = JsonConvert.DeserializeObject<TransactionNinjaModel>(json);
            var transactionInfo = Transaction.Parse(result.Hex);

            result.IsCoinBase = transactionInfo.IsCoinBase;
            result.IsColor = transactionInfo.HasValidColoredMarker();
            result.CleanupOutputs();

            if(result.IsColor && colored == false) return await GetTransactionAsync(txId, true);

            return result;
        }

        public async Task<IDecodetxNinja> DecodeTransactionAsync(string txHex)
        {
            try
            {
                var transactionHex = Transaction.Parse(txHex);

                var relativePath = String.Format("transactions/{0}", txHex);
                var transactionHexUrl = new Uri(Url, relativePath);

                var json = await InvokeMethod(transactionHexUrl);

                return JsonConvert.DeserializeObject<DecodetxModel>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ILastBlockNinja> GetLastBlockAsync()
        {
            var lastBlockUrl = new Uri(Url, "blocks/tip");
            var json = await InvokeMethod(lastBlockUrl);

            return JsonConvert.DeserializeObject<LastBlockModel>(json);
        }

        public async Task<IAddressNinja> GetAddressAsync(string address)
        {
            var relativeUrl = String.Format("whatisit/{0}", address);
            var whatIsItUrl = new Uri(Url, relativeUrl);

            var whatIsItResult = await InvokeMethod(whatIsItUrl);
            var summary = await AddressSummary(address);
            var trList = await AddressListTransaction(address);

            var result = JsonConvert.DeserializeObject<AddressNinjaModel>(whatIsItResult);
            result.Address = address;
            result.Balance = summary.Confirmed.Balance;
            result.TotalTransactions = summary.Confirmed.TotalTransactions;
            result.Assets = summary.Confirmed.Assets;
            result.ListTranasctions = trList.Operations.Select(itm => itm.TxId).ToArray();

            return result;
        }

        private async Task<AddressSummaryConfirmed> AddressSummary(string address)
        {
            var relativeUrl = String.Format("balances/{0}/summary?colored=true", address);
            var addressSumamaryUrl = new Uri(Url, relativeUrl);

            var summary = await InvokeMethod(addressSumamaryUrl);
            return  JsonConvert.DeserializeObject<AddressSummaryConfirmed>(summary);
        }

        private async Task<AddressListTransaction> AddressListTransaction(string address)
        {
            var relativeUrl = String.Format("balances/{0}", address);
            var addressTransactionListUrl = new Uri(Url, relativeUrl);
        
            var trList = await InvokeMethod(addressTransactionListUrl);
            return JsonConvert.DeserializeObject<AddressListTransaction>(trList);
        }

        private async Task<BlockNinjaModel> GetBlockAsync(string block)
        {
            var relativePath = String.Format("blocks/{0}", block);
            var blockUri = new Uri(Url, relativePath);

            var json = await InvokeMethod(blockUri);
            var result = JsonConvert.DeserializeObject<BlockNinjaModel>(json);

            return result;
        }

        public async Task<IBlockNinja> GetInformationBlockAsync(string blockHash)
        {
            var blockResponse = await GetBlockAsync(blockHash);
            var block = Block.Parse(blockResponse.Hex);
            
            var transactionList = GetTransactionsFromBlock(block);

            return BlockNinjaEntity.Create(blockResponse, block, transactionList.ToArray());
        }

        public static IList<ListTransaction> GetTransactionsFromBlock(Block block)
        {
            return block.Transactions.Select(x => new ListTransaction()
            {
                TxId = x.GetHash().ToString(),
                IsColor = x.HasValidColoredMarker()
            }).ToList();
        }

        private static string GetTransactionsFromBlockJson(Block block)
        {
            var transactions = block.Transactions.Select(x => new
            {
                hash = x.GetHash().ToString(),
                isColor = x.HasValidColoredMarker()
            }).ToArray();

            var obj = new
            {
                tr = transactions
            };

            var jsonTransaction = JsonConvert.SerializeObject(obj);

            return jsonTransaction;
        }
    }
}