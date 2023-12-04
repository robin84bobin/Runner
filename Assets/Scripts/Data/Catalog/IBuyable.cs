namespace Data.Catalog
{
    public interface IBuyable
    {
        string Currency { get; }
        int BuyPrice { get; }
    }
}