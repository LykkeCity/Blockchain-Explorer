using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.BitcoinNinja;
using Microsoft.AspNet.Mvc;


namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class AddressController : Controller
    {
        private readonly IBitcoinNinjaClient _bitcoinNinjaReaderRepository;

        public AddressController(IBitcoinNinjaClient bitcoinNinjaReaderRepository)
        {
            _bitcoinNinjaReaderRepository = bitcoinNinjaReaderRepository;
        }

        [HttpGet("/address/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            if(string.IsNullOrEmpty(id))
                    return RedirectToAction("Index", "Home");
            try
            {
                var address = await _bitcoinNinjaReaderRepository.GetAddressAsync(id);
                var model = new AddressViewModel
                {
                    Address = address
                };
                return View(model);
            }
            catch (Exception ex)
            {

                return View("_NotFound");
            }
            
        }
    }
}
