using UnityEngine;

namespace Gameplay.Level.Parts
{
    public class LevelPart: MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        public Vector3 GetSize() => _renderer.bounds.size;

    }
}