using Data.Catalog;
using UnityEngine;

namespace Gameplay.Bonuses
{
    public class BonusController : MonoBehaviour
    {
        public BonusData Data { get; private set; }

        public void Setup(BonusData data)
        {
            Data = data;
        }

        public void OnApplied()
        {
            Destroy(gameObject);
        }
    }
}