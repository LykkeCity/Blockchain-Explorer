using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface IInput
    {
        double Value { get; }
        string Addresses { get; }
        string Txid { get; }
        string BlockHash { get; }
    }

    public class Input : IInput
    {
        public double Value { get; set; }
        public string Addresses { get; set; }
        public string Txid { get; set; }
        public string BlockHash { get; set; }
    }

    public interface IInputsRepository
    {
        Task SaveAsync(Input input);
        Task<IEnumerable<IInput>> GetAsync(string hashBlock);
    }


}