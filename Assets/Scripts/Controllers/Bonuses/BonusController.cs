using UnityEngine;

namespace Controllers.Bonuses
{
    /// <summary>
    /// Component to store bonusId of bonus for processing by collision of gameobject
    /// </summary>
    public class BonusController : MonoBehaviour, IBonusController
    {
        public string BonusId { get; private set; }
        public bool CheckNull()
        {
            return this == null;
        }

        public void Setup(string bonusId)
        {
            BonusId = bonusId;
        }

        public void OnApplied()
        {
            Destroy(gameObject);
        }
    }

    public interface IBonusController
    {
        void Setup(string bonusId);
        string BonusId { get; }

        bool CheckNull();
    }
}