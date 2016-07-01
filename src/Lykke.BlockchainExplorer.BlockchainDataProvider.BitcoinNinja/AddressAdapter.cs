using Lykke.BlockchainExplorer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja.Client;
using Lykke.BlockchainExplorer.Settings;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.BitcoinNinja
{
    public class AddressAdapter : IAddressProvider
    {
        private BitcoinNinjaClient _client;

        public AddressAdapter()
        {
            var settings = new BitcoinNinjaSettings()
            {
                Network = AppSettings.Network,
                UrlMain = AppSettings.SqlNinjaUrlMain,
                UrlTest = AppSettings.SqlNinjaUrlTest
            };

            _client = new BitcoinNinjaClient(settings);
        }
         
        public async Task<Address> GetAddress(string id)
        {
            var a = await _client.GetAddressAsync(id);

            var transactions = a.ListTranasctions.Select(x => new Transaction()
            {
                 TransactionId = x
            }).ToList();

            var assets = a.Assets.Select(x => new Core.Domain.Asset()
            { 
                 Id = x.AssetId,
                 Quantity = x.Quantity,
                 Received = x.Received 
            }).ToList();

            var address = new Address()
            {
                Hash = a.Address,
                ColoredAddress = a.ColoredAddress,
                UncoloredAddress = a.UncoloredAddress,
                Balance = a.Balance,
                TotalTransactions = a.TotalTransactions,
                Transactions = transactions,
                Assets = assets
            };

            return address;
        }
    }
}
