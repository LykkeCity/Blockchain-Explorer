using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Common.Json;
using Core.BitcoinNinja;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class DecodetxController : Controller
    {
        private readonly IBitcoinNinjaReaderRepository _bitcoinNinjaReaderRepository;

        public DecodetxController(IBitcoinNinjaReaderRepository bitcoinNinjaReaderRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return View();

            var model = new DecodetxViewModel
            {
                Decodetx = await _bitcoinNinjaReaderRepository.DecodeTransactionAsync(hex)
            };

            return View(model);
        }
    }
}
