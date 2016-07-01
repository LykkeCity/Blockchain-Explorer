namespace Core
{
    public static class BitcoinUtils
    {
        public static decimal SatoshiToBtc(long satoshi)
        {
            return (decimal) (satoshi * 0.00000001);
        }
    } 
}