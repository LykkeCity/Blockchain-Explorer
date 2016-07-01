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
    public class AddressController : Controller
    {
        public IAddressService AddressService { get; set; }

        private const int ItemsOnPage = 20;

        public AddressController(IAddressService addressService)
        {
            AddressService = addressService;
        }

        [Route("address/{id}")]
        public async Task<ActionResult> Index(string id, int page = 0)

        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Home");
            }

            var address = await AddressService.GetAddress(id);

            if (address == null)
            {
                return View("_NotFound");
            }

            var start = ItemsOnPage * page;

            long max;
            if (start < address.TotalTransactions && start + ItemsOnPage < address.TotalTransactions)
            {
                max = start + ItemsOnPage;
            }
            else
            {
                max = address.TotalTransactions;
            }

            var vm = new AddressModel
            {
                Address = address,
                Count = (int)Math.Ceiling((decimal)address.TotalTransactions / ItemsOnPage),
                CurrentPage = page,
                Start = start,
                Max = max
            };

            return View(vm);
        }
    }
}