namespace Core.BitcoinNinja
{
    public interface IOutputsNinja
    {
        string Address { get; }
        string TxId { get; }
        int Index { get; }
        long Value { get; }
        string AssetId { get; }
        long Quantity { get; }
    }
}