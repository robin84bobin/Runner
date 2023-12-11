using Data.Catalog;

namespace Gameplay
{
    public interface IBonusApplier
    {
        void ApplyBonus(BonusData data);
    }
}