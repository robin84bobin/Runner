using System;
using UnityEngine;

namespace Services.GamePlay.GameplayInput
{
    /// <summary>
    /// provides access to event from concrete input service
    /// </summary>
    public interface IGameInputService
    {
        /// <summary>
        /// event for direction in screen coordinates
        /// </summary>
        event Action<Vector2> OnMoveDirection;
        event Action OnJump;
    }
}