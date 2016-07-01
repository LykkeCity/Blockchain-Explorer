using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Lykke.BlockchainExplorer.UI
{
    public static class Product
    {
        public static string Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fileVersionInfo.ProductVersion;

                return version;
            }
        }
    }
}