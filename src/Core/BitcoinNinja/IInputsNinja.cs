namespace Core.BitcoinNinja
{
    public interface IInputsNinja
    {
        string Address { get; }
        string TxId { get; }
        int Index { get; }
        long Value { get; }
        string AssetId { get; }
        long Quantity { get; }
    }
}