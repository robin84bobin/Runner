using Data.Repository;

namespace Data.Catalog
{
    public class FarmItem : DataItem
    {
        public string ProductId;
        public int ProduceAmount;
        public int ProduceDuration;
        public string ResourceProductId;
        public int ResourceTime;
    }
}