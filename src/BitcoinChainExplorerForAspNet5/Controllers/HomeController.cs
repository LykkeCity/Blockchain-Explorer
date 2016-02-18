using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.Bitcoin;
using Microsoft.AspNet.Mvc;

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILastImportendBlockHash _lastImportendBlockHash;

        public HomeController(ILastImportendBlockHash lastImportendBlockHash)
        {
            _lastImportendBlockHash = lastImportendBlockHash;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexModel
            {
                LastBlock = await _lastImportendBlockHash.GetLastBlock()
            };

            return View(model);
        }

    }
}
