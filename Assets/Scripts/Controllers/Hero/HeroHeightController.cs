using UnityEngine;

namespace Gameplay.Hero
{
    public class HeroHeightController : MonoBehaviour
    {
        private HeroModel _model;
        private Transform _heroTransform;
        
        public void Setup(HeroModel model, float defaultHeight)
        {
            _heroTransform = transform;
            
            _model = model;
            _model.Height.SetDefaultValue(defaultHeight);
            _model.Height.OnValueChange += OnHeightChange;

            SetHeight(_model.Height.DefaultValue);
        }

        private void OnHeightChange(float oldvalue, float newvalue)
        {
            SetHeight(newvalue);
        }

        private void SetHeight(float newvalue)
        {
            var position = _heroTransform.position;
            position = new Vector3(position.x, position.y, -newvalue);
            _heroTransform.position = position;
        }

        private void OnDestroy()
        {
            _model.Height.OnValueChange -= OnHeightChange;
        }
    }
}