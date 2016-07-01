using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lykke.BlockchainExplorer.UI.Controllers
{
    public class SearchController : Controller
    {
        public ISearchService SearchService { get; set; }

        public SearchController(ISearchService searchService)
        {
            SearchService = searchService;
        }

        [Route("search")]
        public async Task<ActionResult> Index(string id)
        {
            var res = await SearchService.SearchEntityById(id);

            if(res == EntitySearchResult.Block) 
            {
                return RedirectToAction("Index", "Block", new { id = id });
            }

            else if(res == EntitySearchResult.Transaction)
            {
                return RedirectToAction("Index", "Transaction", new { id = id });
            }

            else if(res == EntitySearchResult.Address)
            {
                return RedirectToAction("Index", "Address", new { id = id });
            }

            else if(res == EntitySearchResult.Asset)
            {
                return RedirectToAction("Index", "Asset", new { id = id });
            }

            return View("_NotFound");
        }
    }
}