using Commands;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Move
{
    public class MoveCommand : Command
    {
        //move to configs
        private float moveStepValue;
        private float moveStepDuration;
        //
        
        private Transform transform;
        private Vector3 StartPosition;
        private Vector3 Destination; 
        private float startTime; 
        private float moveTime;

        public MoveCommand(Vector3 direction, Transform transform)
        {
            this.transform = transform;
            StartPosition = transform.position;
            Destination = StartPosition + Vector3.Normalize(direction) * moveStepValue;
            startTime = Time.time;
            moveTime = moveStepDuration;
        }


        public override async UniTask Execute()
        {
            while (Destination != transform.position)
            {
                float value = (Time.time - startTime) / moveTime;
                transform.position = Vector3.Lerp(StartPosition, Destination, value);
                await UniTask.Yield();
            }
            
            StopMove();
        }

        private void StopMove()
        {
            Debug.Log($"StopMove : {Time.time - startTime}");
            
            StartPosition = transform.position;
            Destination = StartPosition;
            startTime = 0;
        }
    }
}