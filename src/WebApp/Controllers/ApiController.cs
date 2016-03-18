using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Common.Json;
using Core.Bitcoin;
using Microsoft.AspNet.Mvc;
using Sevices.Bitcoin;


namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class ApiController : Controller
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly ISrvRpcReader _rpcReader;

        public ApiController(ITransactionRepository transactionRepository, ISrvRpcReader rpcReader)
        {
            _transactionRepository = transactionRepository;
            _rpcReader = rpcReader;
        }


        public async Task<IActionResult> Tx(string id)
        {
            if(string.IsNullOrEmpty(id))
                return Json(new ApiTxModel());

            var tx = await _transactionRepository.GetTransaction(id);

            if (tx == null)
                tx = await _rpcReader.GetTransactionByTxIdAsync(id);

            if (tx == null)
                return Json(new ApiTxModel());

            var model = new ApiTxModel
            {
                Txid = tx.Txid,
                Blockhash = tx.Blockhash,
                Time = tx.Time,
                Confirmations = tx.Confirmations,
                Height = tx.Height
            };

            return Json(model);
        }
    }
}
