using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface IAssets
    {
        string AssetId { get; }
        string MetadataUrl { get; }
        string FinalMetadataUrl { get; }
        bool VerifiedIssuer { get; }
        string Name { get; }
        string ContractUrl { get; }
        string NameShort { get; }
        string Issuer { get; }
        string Description { get; }
        string DescriptionMime { get; }
        string Type { get; }
        int Divisibility { get; }
        string IconUrl { get; }
        string ImageUrl { get; }
    }

    public interface IAssetsRepository
    {
        Task<IAssets> GetAssetsDataAsync(string assetId);
        Task WriteAssetsDataAsync(IAssets assetData);
    }

}