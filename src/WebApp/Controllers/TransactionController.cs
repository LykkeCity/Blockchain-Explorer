using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Bitcoin;
using BitcoinChainExplorerForAspNet5.Models;
using Core.BitcoinNinja;
using Microsoft.AspNet.Mvc;

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IBitcoinNinjaReaderRepository _bitcoinNinjaReaderRepository;

        public TransactionController(IBitcoinNinjaReaderRepository bitcoinNinjaReaderRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;

        }

        [HttpGet("/transaction/{id}")]
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("_NotFound");

            var transactin = await _bitcoinNinjaReaderRepository.GetTransactionAsync(id);

            if (transactin == null )
                return View("_NotFound");

            var model = new TransactionViewModel
            {
                Transaction = transactin
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? View("PartialTransactionDetails", model) : View(model);
        }
    }
}