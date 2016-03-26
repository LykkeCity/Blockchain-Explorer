using System;

namespace Core.BitcoinNinja
{
    public interface IBlockNinja
    {
        string Hash { get; }
        long Height { get; }
        DateTime Time { get; }
        long Confirmations { get; }
    }
}