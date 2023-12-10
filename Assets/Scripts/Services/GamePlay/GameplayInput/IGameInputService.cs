using System;
using UnityEngine;

namespace Services.GamePlay.GameplayInput
{
    public interface IGameInputService
    {
        event Action<Vector2> OnMoveDirection;
         Vector2 GetInputMoveDirection();
    }
}