namespace Data.Catalog
{
    public interface ISellable
    {
        string Currency { get; }
        int SellPrice { get; }
    }
}