using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace Gameplay.Spawn
{
    internal class ActorSpawnCommand
    {
        private readonly IResourcesService _resourcesService;
        private readonly string _prefabName;
        private readonly Vector3 _position;
        private readonly Vector3 _rotation;
        private readonly Transform _parent;

        public ActorSpawnCommand(IResourcesService resourcesService, string prefabName, Vector3 position, Vector3 rotation = default,Transform parent = null)
        {
            _resourcesService = resourcesService;
            _prefabName = prefabName;
            _position = position;
            _rotation = rotation;
            _parent = parent;
        }

        public async UniTask Execute()
        {
            var quaternion = Quaternion.Euler(_rotation);
            var r = await _resourcesService.Instantiate(_prefabName, _position, quaternion, _parent);
        }
    }
}