using System;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.Bitcoin;
using Core.BitcoinNinja;
using Microsoft.AspNet.Mvc;


namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class BlockController : Controller
    {
        private readonly IBitcoinNinjaReaderRepository _bitcoinNinjaReaderRepository;
        private readonly IBlockNinjaRepository _blockNinjaRepository;

        private const int ItemsOnPage = 20;

        public BlockController(IBitcoinNinjaReaderRepository bitcoinNinjaReaderRepository, IBlockNinjaRepository blockNinjaRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;
            _blockNinjaRepository = blockNinjaRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id, int page = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return RedirectToAction("Index", "Home");

                var getBlock = await _blockNinjaRepository.GetBlockDataAsync(id);

                if (getBlock == null)
                {
                     getBlock = await _bitcoinNinjaReaderRepository.GetTrGetInformationBlockAsync(id);
                    if (getBlock == null)
                        return View("_NotFound");

                    await _blockNinjaRepository.WriteBlockDataAsync(getBlock);
                }
 
                var start = ItemsOnPage * page;

                int max;
                if (start < getBlock.TotalTransactions && start + ItemsOnPage < getBlock.TotalTransactions)
                {
                    max = start + ItemsOnPage;
                }
                else
                {
                    max = getBlock.TotalTransactions;
                }

                var model = new BlockModel
                {
                    Block = getBlock,
                    Count = (int)Math.Ceiling((decimal)getBlock.TotalTransactions / ItemsOnPage),
                    CurrentPage = page,
                    Start = start,
                    Max = max
                };


                return View(model);
            }
            catch (Exception ex)
            {
                return View("_NotFound");
            }
           

        }

      /*  public async Task<ActionResult> TransactionDetails(string id)
        {
            var trnsct = await _transactionRepository.GetTransaction(id);

            var outputs = await _outputsRepository.GetAsync(trnsct.Blockhash);
            var inputs = await _inputsRepository.GetAsync(trnsct.Blockhash);


            var model = new TransactionModel
            {
               Inputs = inputs.Where(itm => itm.Txid == trnsct.Txid),
               Outputs = outputs.Where(itm => itm.Txid == trnsct.Txid)
            };



            return View(model);
        }*/
    }
}
