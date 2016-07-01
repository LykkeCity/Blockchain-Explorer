using Lykke.BlockchainExplorer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Settings
{
    public class AppSettings
    {
        public static Network Network
        {
            get
            {
                var n = ConfigurationManager.AppSettings[DataKeys.Network];
                return (Network)Enum.Parse(typeof(Network), n);
            }
        }

        public static Uri SqlNinjaUrlMain
        {
            get
            {
                var url = ConfigurationManager.AppSettings[DataKeys.SqlNinjaUrlMain];
                return new Uri(url);
            }
        }

        public static Uri SqlNinjaUrlTest 
        {
            get
            {
                var url = ConfigurationManager.AppSettings[DataKeys.SqlNinjaUrlTest];
                return new Uri(url);
            }
        }

        public static Uri CoinprismUrlTest
        {
            get
            {
                var url = ConfigurationManager.AppSettings[DataKeys.CoinprismUrlTest];
                return new Uri(url);
            }
        }

        public static Uri CoinprismUrlMain
        { 
            get
            {
                var url = ConfigurationManager.AppSettings[DataKeys.CoinprismUrlMain];
                return new Uri(url);
            }
        }

        public static string AzureStorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings[DataKeys.AzureStorageConnectionString];
            }
        }
    }
}