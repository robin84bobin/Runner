using Data.Repository;
using Gameplay;

namespace Data.Catalog
{
    public class AbilityData : DataItem
    {
        public AbilityType abilityType;
        public string title;
        public string description;
        public float value;
        public float duration;
    }
}