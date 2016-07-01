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
        private readonly IBitcoinNinjaClient _bitcoinNinjaReaderRepository;

        public TransactionController(IBitcoinNinjaClient bitcoinNinjaReaderRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;
        }

        [HttpGet("/transaction/{id}")]
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("_NotFound");

            var transaction = await _bitcoinNinjaReaderRepository.GetTransactionAsync(id);

            if (transaction == null )
                return View("_NotFound");

            var model = new TransactionViewModel
            {
                Transaction = transaction
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? View("PartialTransactionDetails", model) : View(model);
        }
    }
}