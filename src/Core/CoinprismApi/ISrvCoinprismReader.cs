using System.Threading.Tasks;
using Core.Bitcoin;
using Newtonsoft.Json;

namespace Core.CoinprismApi
{
    
    public interface ISrvCoinprismReader
    {
        Task<IAssets> GetAssetDataAsync(string assetId);
        Task<IAssetsOwners> GetAssetOwnersDataAsync(string assetId);
    }
}