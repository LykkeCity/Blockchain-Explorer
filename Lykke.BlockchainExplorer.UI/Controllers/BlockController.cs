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
    public class BlockController : Controller
    {
        public IBlockService BlockService { get; set; }

        public BlockController(IBlockService blockService)
        {
            BlockService = blockService;
        }

        private const int ItemsOnPage = 20;

        [Route("block/{id}")]
        public async Task<ActionResult> Index(string id, int page = 0)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Home");
            }
                
            var block = await BlockService.GetBlock(id);

            if(block == null)
            {
                return View("_NotFound");
            }

            var start = ItemsOnPage * page;

            int max;
            if (start < block.TotalTransactions && start + ItemsOnPage < block.TotalTransactions)
            {
                max = start + ItemsOnPage;
            }
            else
            {
                max = block.TotalTransactions; 
            }

            var vm = new BlockModel
            {
                Block = block,
                Count = (int)Math.Ceiling((decimal)block.TotalTransactions / ItemsOnPage),
                CurrentPage = page,
                Start = start,
                Max = max
            };

            return View(vm);
        }
    }
}