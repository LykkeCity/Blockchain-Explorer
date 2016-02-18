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


            var model = new TransactionViewModel
            {
                Transaction = trnsct,
                TransactionsAddreses  =
                    inputs.Where(txId => txId.Txid == trnsct.Txid).GroupBy(itm => itm.Txid)
                        .ToDictionary(itm => itm.Key,
                            itm => new TransactionModel { Inputs = itm, Outputs = new List<IOutput>(), Transactions = new List<ITransaction>() })
            };

            foreach (var output in outputs)
            {
                if (output.Txid == trnsct.Txid)
                {
                    if (!model.TransactionsAddreses.ContainsKey(output.Txid))
                        model.TransactionsAddreses.Add(output.Txid,
                            new TransactionModel { Outputs = new List<IOutput>(), Inputs = new IInput[0], Transactions = new List<ITransaction>() });

                    model.TransactionsAddreses[output.Txid].Outputs.Add(output);
                    model.Fee = model.TransactionsAddreses[output.Txid].Fee;
                }
                
            }


            return View(model);
        }
    }
}