using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.Bitcoin;
using Core.BitcoinNinja;
using Microsoft.AspNet.Mvc;

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBitcoinNinjaClient _bitcoinNinjaReaderRepository;


        public HomeController(IBitcoinNinjaClient bitcoinNinjaReaderRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexModel
            {
                LastBlock = await _bitcoinNinjaReaderRepository.GetLastBlockAsync()
            };

            return View(model);
        }


       

    }
}
