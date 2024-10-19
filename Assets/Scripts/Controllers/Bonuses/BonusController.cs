using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Bonuses
{
    /// <summary>
    /// Component to store bonusId of bonus for processing by collision of gameobject
    /// </summary>
    public class BonusController : MonoBehaviour, IBonusController
    {
        [SerializeField] private float dissolveSpeed = .01f;
        public string BonusId { get; private set; }
        private const string Dissolve = "_Dissolve";
        
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
            StartCoroutine( ShowDissolveCoroutine());
        }

        private IEnumerator ShowDissolveCoroutine()
        {
            var mesh = GetComponentInChildren<MeshRenderer>();
            WaitForSeconds dissolveWaitForSeconds = new WaitForSeconds(dissolveSpeed);
            for (float value = 0; value < 1; value += 0.01f)
            {
                if (mesh == null)
                    yield break;
                
                mesh.material.SetFloat(Dissolve, value);
               
                yield return dissolveWaitForSeconds;
            }
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