using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.GameplayInput
{
    public class MobileGameInputService : IGameInputService, IDisposable
    {
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
            _startTouchTime = Time.time;
            _startTouchPosition = _inputActions.Mobile.PrimaryTouchPosition.ReadValue<Vector2>();
        }

        private void OnPrimaryTouchEnd(InputAction.CallbackContext context)
        {
            _endTouchTime = Time.time;
            _endTouchPosition = _inputActions.Mobile.PrimaryTouchPosition.ReadValue<Vector2>();
        }

        public Vector2 GetInputMoveDirection()
        {
            _swipeDirection = Vector2.zero;
            
            if (_endTouchTime - _startTouchTime > 1f)
                return default;

            if (Vector2.Distance(_endTouchPosition, _startTouchPosition) < 0.2f)
                return default;

            _swipeDirection = _endTouchPosition - _startTouchPosition;
            
            _startTouchTime = 0f;
            _endTouchTime = 0f;
            _startTouchPosition = Vector2.zero;
            _endTouchPosition = Vector2.zero;
            
            return _swipeDirection;
        }

        public bool GetJump()
        {
            var value = _inputActions.Mobile.PrimaryTouchContact.ReadValue<float>();
            return value != 0;
        }

        public void Dispose()
        {
            _inputActions.Mobile.PrimaryTouchContact.started -= OnPrimaryTouchStart;
            _inputActions.Mobile.PrimaryTouchContact.canceled -= OnPrimaryTouchEnd;
            _inputActions?.Dispose();
        }
    }
}