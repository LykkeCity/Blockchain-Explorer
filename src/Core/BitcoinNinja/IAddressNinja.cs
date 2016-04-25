using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.BitcoinNinja
{
    public interface IAddressNinja
    {
        string Address { get; }
        string ColoredAddress { get; }
        string UncoloredAddress { get; }
        long Balance { get; }
        int TotalTransactions { get; }
        AssetsForAddress[] Assets { get; }
        string[] ListTranasctions { get; }
    }


}