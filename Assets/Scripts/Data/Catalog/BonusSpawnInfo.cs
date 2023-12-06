namespace Data.Catalog
{
    public class BonusSpawnInfo : IWeightable
    {
        public string id;
        public int weight;
        public int Weight => weight;
    }
}