using Cysharp.Threading.Tasks;
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

        public async UniTask OnApplied()
        {
            var mesh = GetComponentInChildren<MeshRenderer>();
            
            for (float value = 0; value < 1; value += 0.01f)
            {
                if (mesh == null)
                    return;
                mesh.material.SetFloat("_Dissolve", value);
                await UniTask.Yield();
            }
        }
    }

    public interface IBonusController
    {
        void Setup(string bonusId);
        string BonusId { get; }

        bool CheckNull();
    }
}