namespace Data.Catalog
{
    public class PartSpawnInfo : IWeightable
    {
        public string id;
        public int weight;
        public int Weight => weight;
    }
}