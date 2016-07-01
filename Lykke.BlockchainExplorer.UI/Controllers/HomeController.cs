using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lykke.BlockchainExplorer.UI.Controllers
{
    public class HomeController : Controller
    {
        public IBlockService BlockService { get; set; }

        public HomeController(IBlockService blockService)
        {
            BlockService = blockService;
        }

        public async Task<ActionResult> Index()
        {
            var lastBlock = await BlockService.GetLastBlock();

            var vm = new IndexModel()
            {
                LastBlock = lastBlock,
            };

            return View(vm);
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}