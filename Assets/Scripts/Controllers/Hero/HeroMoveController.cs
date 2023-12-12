using System.Collections;
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
    public class HeroMoveController : MonoBehaviour
    {
        private bool _isMoving = false;
        private Coroutine _moveCoroutine;
        private float _moveValue;
        
        private float _leftXBound;
        private float _rightXBound;
        private Vector3 _startPosition;
        private float _startTime;
        
        private IGameInputService _inputService;
        private GameplayConfig _config;
        
        private HeroModel _model;
        private Transform _heroTransform;

        public void Setup(GameplayConfig config, IGameInputService inputService, Transform moveBoundaries, HeroModel model)
        {
            _model = model;
            _inputService = inputService;
            _config = config;

            _heroTransform = transform;
            _model.Height.SetDefaultValue(-_heroTransform.position.z);
            _model.Height.OnValueChange += OnHeightChange;

            SetHeight(_model.Height.DefaultValue);
            SetupMoveRestrictions(moveBoundaries);
            
            _inputService.OnMoveDirection += ProcessMove;
        }
        
        private void SetupMoveRestrictions(Transform moveBoundaries)
        {
            var boxCollider = moveBoundaries.GetComponent<BoxCollider>();
            _moveValue = boxCollider.size.x / _config.RunTrailCount;
            
            _leftXBound = moveBoundaries.transform.TransformPoint(boxCollider.center - boxCollider.size * 0.5f).x;
            _rightXBound = moveBoundaries.transform.TransformPoint(boxCollider.center + boxCollider.size * 0.5f).x;
        }
        
        private void ProcessMove(Vector2 inputMoveDirection)
        {
            if (_isMoving)
                return;

            //TODO why hero position is not on the edge of collider this way???
            var moveDirection = inputMoveDirection.NormalizeToHorizontalDirection(_moveValue);
            _startTime = Time.time;
            _startPosition = transform.position;
            Vector3 destination = _startPosition + new Vector3(moveDirection.x, moveDirection.y);

            if (destination.x > _rightXBound || destination.x < _leftXBound)
                return;
            
            MoveAsyncTo(destination, _config.MoveTime);
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

        private IEnumerator MoveTo(Vector3 destination, float moveTime)
        {
            _isMoving = true;
            while (destination != transform.position)
            {
                float value = (Time.time - _startTime) / moveTime;
                transform.position = Vector3.Lerp(_startPosition, destination, value);
                yield return null;
            }
            _isMoving = false;
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
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            _inputService.OnMoveDirection -= ProcessMove;
            _model.Height.OnValueChange -= OnHeightChange;
            _isMoving = false;
        }
    }
}