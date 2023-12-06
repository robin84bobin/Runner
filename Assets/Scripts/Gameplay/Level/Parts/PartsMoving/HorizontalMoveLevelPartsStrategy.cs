using System.Collections.Generic;
using UnityEngine;

public class HorizontalMoveLevelPartsStrategy : IMoveLevelPartsStrategy
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
        return new Vector3(- bounds.size.x, 0,  0);
    }

    public Vector3 GetNewPartPosition(LevelPart newPart, List<LevelPart> currentParts)
    {
        var partSize = newPart.GetSize().x;
        float nextPartPosX = _container.position.x;
        
        if (currentParts.Count > 0)
        {
            var lastPart = currentParts[currentParts.Count - 1];
            var lastPartPos = lastPart.transform.position.x;
            nextPartPosX = lastPartPos + partSize;
        }
        
        return new Vector3(nextPartPosX, _container.position.y, _container.position.z); 
    }

    public bool CheckRemovePart(LevelPart part) => part.transform.position.x < _partRemoveBound.x;
    
    public void Move(LevelPart part, float speed)
    {
        part.transform.position += new Vector3(speed, 0f, 0f);
    }
}