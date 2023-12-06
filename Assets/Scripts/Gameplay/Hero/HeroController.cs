using UnityEngine;

namespace Gameplay.Hero
{
    internal class HeroController : MonoBehaviour
    {
        private HeroModel _model;

        public void Setup(HeroModel model)
        {
            _model = model;
        }
    }
}