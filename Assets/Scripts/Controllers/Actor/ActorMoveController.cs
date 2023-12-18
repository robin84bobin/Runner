using System;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using Model;
using Services.GamePlay.GameplayInput;
using UnityEngine;

namespace Controllers.Hero
{
    /// <summary>
    /// Applies hero moving via events from model and input
    /// </summary>
    public class ActorMoveController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly int StrafeLeftAnimParam = Animator.StringToHash("StrafeLeft");
        private readonly int StrafeRightAnimParam = Animator.StringToHash("StrafeRight");
        private readonly int SpeedAnimParam = Animator.StringToHash("Speed");
        private readonly int HeightAnimParam = Animator.StringToHash("Height");
        
        private bool _isStrafeMoving = false;
        private float _moveValue;
        
        private float _leftXBound;
        private float _rightXBound;
        private Vector3 _strafeStartPosition;
        private float _strafeStartTime;
        
        private IGameInputService _inputService;
        private GameplayConfig _config;
        
        private ActorModel _model;
        private Transform _actorTransform;
        
        private CancellationTokenSource _heightCancellationTokenSource;
        
        public void Setup(GameplayConfig config, IGameInputService inputService, Transform moveBoundaries, ActorModel model)
        {
            _model = model;
            _inputService = inputService;
            _config = config;

            _actorTransform = transform;
            _model.Height.SetDefaultValue(-_actorTransform.position.z);
            _model.Height.OnValueChange += OnHeightChange;
            _model.Speed.OnValueChange += OnSpeedChange;
            
            SetupMoveRestrictions(moveBoundaries);

            SetHeight(_model.Height.DefaultValue);
            SetSpeed(_model.Speed.DefaultValue);

            _inputService.OnMoveDirection += ProcessStrafe;
            _inputService.OnJump += TestDropHeight;
        }

        private void TestDropHeight()
        {
            _model.Height.ResetToDefaultValue();
        }

        private void OnSpeedChange(float oldvalue, float value)
        {
            SetSpeed(value);
        }

        private void SetSpeed(float value)
        {
            animator.SetFloat(SpeedAnimParam, value);
        }

        private void SetupMoveRestrictions(Transform moveBoundaries)
        {
            var boxCollider = moveBoundaries.GetComponent<BoxCollider>();
            _moveValue = boxCollider.size.x / _config.RunTrailCount;
            
            _leftXBound = moveBoundaries.transform.TransformPoint(boxCollider.center - boxCollider.size * 0.5f).x;
            _rightXBound = moveBoundaries.transform.TransformPoint(boxCollider.center + boxCollider.size * 0.5f).x;
        }
        
        private async void ProcessStrafe(Vector2 inputMoveDirection)
        {
            if (_isStrafeMoving)
                return;

            var moveDirection = inputMoveDirection.NormalizeToHorizontalDirection(_moveValue);
            _strafeStartTime = Time.time;
            _strafeStartPosition = transform.position;
            Vector3 destination = _strafeStartPosition + new Vector3(moveDirection.x, 0);

            if (destination.x > _rightXBound || destination.x < _leftXBound)
                return;

            int strafeAnimParam = inputMoveDirection.x < 0 ? StrafeLeftAnimParam : StrafeRightAnimParam;
            animator.SetBool(strafeAnimParam, true);
            
            await MoveAsyncTo(destination, _config.StrafeTime);
            
            animator.SetBool(strafeAnimParam, false);
        }

        private async UniTask MoveAsyncTo(Vector3 destination, float moveTime)
        {
            _isStrafeMoving = true;
            while (destination.x != _actorTransform.position.x)
            {
                float progress = (Time.time - _strafeStartTime) / moveTime;
                _actorTransform.position = Vector3.Lerp(_strafeStartPosition, destination, progress);
                await UniTask.Yield();
            }
            _isStrafeMoving = false;
        }

        private void OnHeightChange(float oldvalue, float newvalue)
        {
            _heightCancellationTokenSource?.Cancel();
            
            _heightCancellationTokenSource = new CancellationTokenSource();
            _heightCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(_config.MoveVerticalTime * 2));
            SetHeightAsync(newvalue, _config.MoveVerticalTime, _heightCancellationTokenSource.Token);
        }

        private void SetHeight(float value)
        {
            animator.SetFloat(HeightAnimParam, value);
            var position = _actorTransform.position;
            _actorTransform.position = new Vector3(position.x, position.y, - value);
        }
        
        private async UniTask SetHeightAsync(float value, float moveTime, CancellationToken cancellationToken)
        {
            var position = _actorTransform.position;
            var endHeight = -value;

            var startHeight = position.z;
            float startTime = Time.time;
            
            while (Math.Abs(endHeight - _actorTransform.position.z) > 0 )
            {
                float progress = (Time.time - startTime) / moveTime;
                float height =  Mathf.Lerp(startHeight, endHeight, progress);
                animator.SetFloat(HeightAnimParam, -height);
                position = _actorTransform.position;
                _actorTransform.position = new Vector3(position.x, position.y, height);
                await UniTask.Yield(cancellationToken);
            }
            animator.SetFloat(HeightAnimParam, value);
        }
        
        private void OnDestroy()
        {
            _inputService.OnJump -= TestDropHeight;
            _inputService.OnMoveDirection -= ProcessStrafe;
            _model.Height.OnValueChange -= OnHeightChange;
            _model.Speed.OnValueChange -= OnSpeedChange;
            _isStrafeMoving = false;
        }
    }
}