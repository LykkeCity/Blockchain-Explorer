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
    public class TransactionController : Controller
    {
        public ITransactionService TransactionService { get; set; }

        public TransactionController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        [Route("transaction/{id}")]
        public async Task<ActionResult> Index(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Home");
            }

            var vm = await getTransactionVm(id); 

            if (vm.Transaction == null)
            {
                return View("_NotFound");
            }

            return View(vm);
        }

        public async Task<ActionResult> PartialTransactionDetails(string id)
        {
            var vm = await getTransactionVm(id);

            if (vm.Transaction == null)
            {
                return View("_NotFound");
            } 

            return View(vm);
        }

        private async Task<TransactionModel> getTransactionVm(string transactionId)
        {
            var transaction = await TransactionService.GetTransaction(transactionId);

            var vm = new TransactionModel()
            {
                Transaction = transaction
            };

            return vm;
        }
    }
}