using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface ILastBlock
    {
        string Hash { get; }
        long Height { get; }
    }

    public interface ILastImportendBlockHash
    {
        Task<string> GetAsync();
        Task SetAsync(string hash, long height);
        Task<ILastBlock> GetLastBlock();
    }
}
