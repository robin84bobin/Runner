using System;
using UnityEngine;
using Zenject;

namespace Services.GamePlay.GameplayInput
{
    /// <summary>
    /// detects input axis directions  
    /// </summary>
    public class StandaloneGameInputService : IGameInputService, ITickable
    {
        public event Action<Vector2> OnMoveDirection;

        public void Tick()
        {
            ReadInputValues();
        }

        private void ReadInputValues()
        {
            Vector2 vector = GetInputMoveDirection();
            
            if (vector.Equals(Vector2.zero))
                return;
            
            OnMoveDirection?.Invoke(vector);
        }

        private Vector2 GetInputMoveDirection()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}