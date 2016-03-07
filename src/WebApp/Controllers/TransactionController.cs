using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Bitcoin;
using BitcoinChainExplorerForAspNet5.Models;
using Microsoft.AspNet.Mvc;

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOutputsRepository _outputsRepository;
        private readonly IInputsRepository _inputsRepository;

        public TransactionController(ITransactionRepository transactionRepository, IOutputsRepository outputsRepository, IInputsRepository inputsRepository)
        {
            _transactionRepository = transactionRepository;
            _outputsRepository = outputsRepository;
            _inputsRepository = inputsRepository;
        }

        [HttpGet("/transaction/{id}")]
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("_NotFound");

            var trnsct = await _transactionRepository.GetTransaction(id);

            if (trnsct == null )
                return View("_NotFound");

            var outputs = await _outputsRepository.GetAsync(trnsct.Blockhash);
            var inputs = await _inputsRepository.GetAsync(trnsct.Blockhash);


            var model = new TransactionModel
            {
                Inputs = inputs.Where(itm => itm.Txid == trnsct.Txid),
                Outputs = outputs.Where(itm => itm.Txid == trnsct.Txid),
                Transaction = trnsct
            };


            return View(model);
        }
    }
}