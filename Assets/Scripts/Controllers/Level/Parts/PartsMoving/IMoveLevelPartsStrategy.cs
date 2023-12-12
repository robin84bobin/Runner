using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Level.Parts.PartsMoving
{
    /// <summary>
    /// provides methods to get values to move level parts
    /// </summary>
    public interface IMoveLevelPartsStrategy
    {
        void Init(Transform container);
        Vector3 GetNewPartPosition(LevelPart newPart, List<LevelPart> currentParts);
        bool CheckRemovePart(LevelPart part);
        void Move(LevelPart part, float speed);
    }
}