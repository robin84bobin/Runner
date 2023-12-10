using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Services.GamePlay.GameplayInput
{
    public class MobileGameInputService : IGameInputService, ITickable, IDisposable
    {
        const float SwipeTimeThreshold = 1f;
        const float SwipeDistanceThreshold = 0.8f;
        public event Action<Vector2> OnMoveDirection;
        
        private readonly InputActions _inputActions;
        private Vector2 _swipeDirection = new Vector2();
        private float _startTouchTime;
        private float _endTouchTime;
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        public MobileGameInputService()
        {
            _inputActions = new InputActions();
            _inputActions.Mobile.Enable();

            _inputActions.Mobile.PrimaryTouchContact.started += OnPrimaryTouchStart;
            _inputActions.Mobile.PrimaryTouchContact.canceled += OnPrimaryTouchEnd;
        }

        private void OnPrimaryTouchStart(InputAction.CallbackContext context)
        {
            DropValues();
            _startTouchTime = Time.time;
            _startTouchPosition = _inputActions.Mobile.PrimaryTouchPosition.ReadValue<Vector2>();
        }

        private void OnPrimaryTouchEnd(InputAction.CallbackContext context)
        {
            _endTouchTime = Time.time;
            _endTouchPosition = _inputActions.Mobile.PrimaryTouchPosition.ReadValue<Vector2>();

            if (_endTouchTime - _startTouchTime > SwipeTimeThreshold)
            {
                DropValues();
                return;
            }

            var distance = Vector2.Distance(_endTouchPosition, _startTouchPosition);
            if (distance < SwipeDistanceThreshold)
            {
                DropValues();
                return;
            }

            _swipeDirection = _endTouchPosition - _startTouchPosition;
        }


        public Vector2 GetInputMoveDirection()
        {
            return _swipeDirection;
        }

        private void DropValues()
        {
            _startTouchTime = default;
            _endTouchTime = default;
            _startTouchPosition = default;
            _endTouchPosition = default;
            _swipeDirection = default;
        }

        public void Tick()
        {
            var inputMoveDirection = GetInputMoveDirection();
            if (inputMoveDirection == default) 
                return;
            
            OnMoveDirection?.Invoke(inputMoveDirection);
            DropValues();
        }

        public void Dispose()
        {
            _inputActions.Mobile.PrimaryTouchContact.started -= OnPrimaryTouchStart;
            _inputActions.Mobile.PrimaryTouchContact.canceled -= OnPrimaryTouchEnd;
            _inputActions?.Dispose();
        }
    }
}