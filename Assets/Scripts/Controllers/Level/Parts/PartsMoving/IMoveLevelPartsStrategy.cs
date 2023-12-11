using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level.Parts.PartsMoving
{
    public interface IMoveLevelPartsStrategy
    {
        void Init(Transform container);
        Vector3 GetNewPartPosition(LevelPart newPart, List<LevelPart> currentParts);
        bool CheckRemovePart(LevelPart part);
        void Move(LevelPart part, float speed);
    }
}