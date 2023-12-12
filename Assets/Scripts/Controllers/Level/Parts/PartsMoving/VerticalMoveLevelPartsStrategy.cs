using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Level.Parts.PartsMoving
{
    /// <summary>
    /// calculates values to move level parts in vertical direction
    /// </summary>
    public class VerticalMoveLevelPartsStrategy : IMoveLevelPartsStrategy
    {
        private Transform _container;
        private Vector3 _partRemoveBound;

        public void Init(Transform container)
        {
            _container = container;
            _partRemoveBound = GetPartRemoveBound();
        }

        private Vector3 GetPartRemoveBound()
        {
            var bounds = _container.GetComponent<BoxCollider>().bounds;
            return new Vector3(0, - bounds.size.y, 0);
        }

        public Vector3 GetNewPartPosition(LevelPart newPart, List<LevelPart> currentParts)
        {
            var partSize = newPart.GetSize().y;
            float nextPartPosY = _container.position.y;
        
            if (currentParts.Count > 0)
            {
                var lastPart = currentParts[currentParts.Count - 1];
                var lastPartPos = lastPart.transform.position.y;
                nextPartPosY = lastPartPos + partSize;
            }
        
            return new Vector3(_container.position.x, nextPartPosY, _container.position.z); 
        }

        public bool CheckRemovePart(LevelPart part) => part.transform.position.y < _partRemoveBound.y;
    
        public void Move(LevelPart part, float speed)
        {
            part.transform.position -= new Vector3(0f, speed, 0f);
        }
    }
}