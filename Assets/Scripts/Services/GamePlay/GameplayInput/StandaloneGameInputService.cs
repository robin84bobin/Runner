using System;
using UnityEngine;

namespace Services.GamePlay.GameplayInput
{
    public class StandaloneGameInputService : IGameInputService
    {
        public event Action<Vector2> OnMoveDirection;
        public Vector2 GetInputMoveDirection() => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public void Tick()
        {
            ReadInputValues();
        }

        public void LateTick()
        {
            DropInputValues();
        }

        private void ReadInputValues()
        {
            //TODO
        }

        private void DropInputValues()
        {
            //TODO
        }
    }
}