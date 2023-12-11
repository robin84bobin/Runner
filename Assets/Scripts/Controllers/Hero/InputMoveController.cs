using System.Collections;
using Common;
using Services.GamePlay.GameplayInput;
using UnityEngine;

namespace Gameplay.Hero
{
    public class InputMoveController : MonoBehaviour
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
        
        public void Setup(GameplayConfig config, IGameInputService inputService, Transform moveBoundaries)
        {
            _inputService = inputService;
            _config = config;

            SetupMoveRestrictions(moveBoundaries);
            
            _inputService.OnMoveDirection += ProcessMove;
        }
        
        //TODO calc restrictions outside
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
            
            _moveCoroutine = StartCoroutine(MoveTo(destination, _config.MoveTime));            
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
        
        private void OnDestroy()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            _inputService.OnMoveDirection -= ProcessMove;
            _isMoving = false;
        }
    }
}