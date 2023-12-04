using Data.Repository;

namespace Data.Catalog
{
    public class Product : DataItem, ISellable
    {
        public string Currency { get; set; }
        public int SellPrice { get; set; }
    }
}