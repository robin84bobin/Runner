﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Services.GamePlay.GameplayInput
{
    /// <summary>
    /// detects swipes direction  
    /// </summary>
    public class MobileGameInputService : IGameInputService, ITickable, IDisposable
    {
        private readonly GameplayConfig _gameplayConfig;
        public event Action<Vector2> OnMoveDirection;
        public event Action OnJump;

        private readonly InputActions _inputActions;
        private Vector2 _swipeDirection = new Vector2();
        private float _startTouchTime;
        private float _endTouchTime;
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        public MobileGameInputService(GameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;
            _inputActions = new InputActions();
            _inputActions.Mobile.Enable();

            _inputActions.Mobile.PrimaryTouchContact.started += OnPrimaryTouchStart;
            _inputActions.Mobile.PrimaryTouchContact.canceled += OnPrimaryTouchEnd;
        }

        void ITickable.Tick()
        {
            if (_swipeDirection == default) 
                return;
            
            OnMoveDirection?.Invoke(_swipeDirection);
            DropValues();
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

            if (CheckSwipeDetected() == false)
            {
                DropValues();
                return;
            }

            _swipeDirection = _endTouchPosition - _startTouchPosition;
        }


        private bool CheckSwipeDetected()
        {
            var distance = Vector2.Distance(_endTouchPosition, _startTouchPosition);
            return _endTouchTime - _startTouchTime < _gameplayConfig.SwipeTimeThreshold ||
                   distance > _gameplayConfig.SwipeDistanceThreshold;
        }

        private void DropValues()
        {
            _startTouchTime = default;
            _endTouchTime = default;
            _startTouchPosition = default;
            _endTouchPosition = default;
            _swipeDirection = default;
        }

        void IDisposable.Dispose()
        {
            _inputActions.Mobile.PrimaryTouchContact.started -= OnPrimaryTouchStart;
            _inputActions.Mobile.PrimaryTouchContact.canceled -= OnPrimaryTouchEnd;
            _inputActions?.Dispose();
        }
    }
}