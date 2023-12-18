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
        
        private bool _isMoving = false;
        private Coroutine _moveCoroutine;
        private float _moveValue;
        
        private float _leftXBound;
        private float _rightXBound;
        private Vector3 _startPosition;
        private float _startTime;
        
        private IGameInputService _inputService;
        private GameplayConfig _config;
        
        private ActorModel _model;
        private Transform _heroTransform;

        public void Setup(GameplayConfig config, IGameInputService inputService, Transform moveBoundaries, ActorModel model)
        {
            _model = model;
            _inputService = inputService;
            _config = config;

            _heroTransform = transform;
            _model.Height.SetDefaultValue(-_heroTransform.position.z);
            _model.Height.OnValueChange += OnHeightChange;
            _model.Speed.OnValueChange += OnSpeedChange;
            
            SetupMoveRestrictions(moveBoundaries);

            SetHeight(_model.Height.DefaultValue);
            SetSpeed(_model.Speed.DefaultValue);

            _inputService.OnMoveDirection += ProcessMove;
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
        
        private async void ProcessMove(Vector2 inputMoveDirection)
        {
            if (_isMoving)
                return;

            var moveDirection = inputMoveDirection.NormalizeToHorizontalDirection(_moveValue);
            _startTime = Time.time;
            _startPosition = transform.position;
            Vector3 destination = _startPosition + new Vector3(moveDirection.x, moveDirection.y);

            if (destination.x > _rightXBound || destination.x < _leftXBound)
                return;

            int strafeAnimParam = inputMoveDirection.x < 0 ? StrafeLeftAnimParam : StrafeRightAnimParam;
            animator.SetBool(strafeAnimParam, true);
            
            await MoveAsyncTo(destination, _config.MoveTime);
            
            animator.SetBool(strafeAnimParam, false);
        }

        private async UniTask MoveAsyncTo(Vector3 destination, float moveTime)
        {
            _isMoving = true;
            while (destination != transform.position)
            {
                float value = (Time.time - _startTime) / moveTime;
                transform.position = Vector3.Lerp(_startPosition, destination, value);
                await UniTask.Yield();
            }
            _isMoving = false;
        }

        private void OnHeightChange(float oldvalue, float newvalue)
        {
            SetHeight(newvalue);
        }

        private void SetHeight(float value)
        {
            var position = _heroTransform.position;
            position = new Vector3(position.x, position.y, -value);
            _heroTransform.position = position;
            
            animator.SetFloat(HeightAnimParam, value);
        }
        
        private void OnDestroy()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            _inputService.OnMoveDirection -= ProcessMove;
            _model.Height.OnValueChange -= OnHeightChange;
            _model.Speed.OnValueChange -= OnSpeedChange;
            _isMoving = false;
        }
    }
}