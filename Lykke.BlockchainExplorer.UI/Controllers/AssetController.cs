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
    public class AssetController : Controller
    {
        public IAssetService AssetService { get; set; }

        public AssetController(IAssetService assetService)
        {
            AssetService = assetService;
        }

        [Route("asset/{id}")]
        public async Task<ActionResult> Index(string id)
        {
            var asset = await AssetService.GetAsset(id); 

            if (asset == null)
            {
                return View("_NotFound");
            }

            var vm = new AssetModel()
            {
                Asset = asset
            };

            return View(vm);
        }

        [Route("asset/directory")]
        public async Task<ActionResult> AssetDirectory()
        {
            var assets = await AssetService.GetAssetList();

            var vm = new AssetDirectoryModel()
            {
                Assets = assets
            };

            return View(vm);
        }

        [Route("asset/owners/{id}")]
        public async Task<ActionResult> AssetOwners(string id)
        {
            var assetOwners = await AssetService.GetAssetOwners(id);

            if (assetOwners == null)
            {
                return View("_NotFound");
            }

            var vm = new AssetOwnersModel()
            {
                Asset = assetOwners.Asset,
                AssetOwners = assetOwners,
                Total = assetOwners.Owners.Sum(x => x.Quantity)
            };

            return View(vm);
        }
    }
}