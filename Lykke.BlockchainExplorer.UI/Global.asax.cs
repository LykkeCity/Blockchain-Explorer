using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lykke.BlockchainExplorer.ServiceLocator;
using Lykke.BlockchainExplorer.Core.Log;

namespace Lykke.BlockchainExplorer.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            var log = ServiceLocator.ServiceLocator.Resolve<ILog>();
            log.WriteFatalError("GlobalError", "ApplicationError", exception.Message, exception, DateTime.Now);

            Response.Redirect("/Home/Error");
        }
    }
}
