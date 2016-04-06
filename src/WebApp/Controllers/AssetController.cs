using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.Bitcoin;
using Core.CoinprismApi;
using Microsoft.AspNet.Mvc;
using Sevices.CoinprismApi.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly ISrvCoinprismReader _coinprismReader;
        private readonly IAssetsOwnersRepository _assetsOwnersRepository;

        public AssetController(IAssetsRepository assetsRepository, ISrvCoinprismReader coinprismReader, IAssetsOwnersRepository assetsOwnersRepository)
        {
            _assetsRepository = assetsRepository;
            _coinprismReader = coinprismReader;
            _assetsOwnersRepository = assetsOwnersRepository;
        }

        [HttpGet("/asset/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var assetData = await _assetsRepository.GetAssetsDataAsync(id);
            if (assetData == null)
            {
                assetData = await _coinprismReader.GetAssetDataAsync(id);
                await _assetsRepository.WriteAssetsDataAsync(assetData);
            }

            var model = new AssetsViewModel
            {
                Asset = assetData
            };

            return View(model);
        }

        [HttpGet("/asset/owners/{id}")]
        public async Task<IActionResult> Owners(string id)
        {
  
            var assetOwnersData = await _assetsOwnersRepository.GetAssetsOwnersDataAsync(id);
            if (assetOwnersData == null)
            {
               var assetOwnersDataConprism = await _coinprismReader.GetAssetOwnersDataAsync(id);
                await _assetsOwnersRepository.WriteAssetsOwnersDataAsync(assetOwnersDataConprism);
                assetOwnersData = await _assetsOwnersRepository.GetAssetsOwnersDataAsync(id);
            }

            var assetData = await _assetsRepository.GetAssetsDataAsync(id);
            if (assetData == null)
            {
                assetData = await _coinprismReader.GetAssetDataAsync(id);
                await _assetsRepository.WriteAssetsDataAsync(assetData);
            }

            var model = new AssetsOwnersViewModel
            {
              AssetsOwners  = assetOwnersData,
              Total = assetOwnersData.Owners.Sum(itm => itm.AssetQuantity),
              Asset = assetData
            };

            return View(model);
        }


    }
}
