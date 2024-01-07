using Core.Data.Repository;
using Model.Abilities;
using Parameters;

namespace Data.Catalog
{
    public class AbilityData : DataItem
    {
        public AbilityType abilityType;
        public AbilityParamAction paramAction = AbilityParamAction.Add;
        public ParamName paramType;
        public string title;
        public string description;
        public float value;
        public float duration;
    }
}