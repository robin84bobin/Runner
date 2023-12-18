using System;
using System.Collections.Generic;
using Controllers.Level.Parts.PartsMoving;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Model;
using Services.GamePlay;
using Services.Resources;
using UnityEngine;
using Zenject;

namespace Controllers.Level.Parts
{
    /// <summary>
    /// controls spawning and moving of level parts
    /// spawning bonuses etc.
    /// </summary>
    public class LevelController : IFixedTickable, IDisposable
    {
        private readonly IResourcesService _resourcesService;
        private readonly GameLevelService _gameLevelService;
        private readonly IMoveLevelPartsStrategy _moveStrategy;
        private readonly GameplayConfig _config;
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly ActorModel _actorModel;

        private Transform _container;

        private PartSpawnInfo[] _partInfos;
        private List<LevelPart> _currentParts;

        int _lastPartIndex = -1;
        bool IsInitialized = false;
        private PartSpawnInfo[] _partSpawnInfos;

        private float MoveSpeed => _actorModel.Speed.Value;

        public LevelController(
            GameModel gameModel,
            IResourcesService resourcesService, 
            GameLevelService gameLevelService, 
            IMoveLevelPartsStrategy moveStrategy,
            GameplayConfig config,
            CatalogDataRepository catalogDataRepository
        )
        {
            _actorModel = gameModel.HeroModel;
            _resourcesService = resourcesService;
            _gameLevelService = gameLevelService;
            _moveStrategy = moveStrategy;
            _config = config;
            _catalogDataRepository = catalogDataRepository;
        }

        public async UniTask CreateParts(PartSpawnInfo[] partSpawnInfos, Transform container)
        {
            _container = container;
            _moveStrategy.Init(_container);
            _partInfos = partSpawnInfos;
        
            _currentParts = new List<LevelPart>();

            for (int i = 0; i < _config.VisibleLevelPartCount; i++)
            {
                var part = NextPart();
                await SpawnPart(part);
            }

            IsInitialized = true;
        }

        void IFixedTickable.FixedTick()
        {
            if (!IsInitialized) 
                return;

            for (int i = 0; i < _currentParts.Count; i++)
            {
                var part = _currentParts[i];
                _moveStrategy.Move(part, MoveSpeed);
                TryRemovePart(part);
            }
            TryAddPart();
        }

        private void TryAddPart()
        {
            if (_currentParts.Count >= _config.VisibleLevelPartCount) 
                return;
        
            var part = NextPart();
            SpawnPart(part);
        }

        private async UniTask SpawnPart(PartSpawnInfo partSpawnInfo)
        {
            var partAssetKey = _gameLevelService.GetPartPrefabName(partSpawnInfo.id);
           
            var partGO = await _resourcesService.Instantiate( 
                partAssetKey, 
                Vector3.zero, 
                _container.rotation, 
                _container
            );
            
            LevelPart newPart = partGO.GetComponent<LevelPart>();

            var bonuses = _gameLevelService.LevelData.bonuses;
            await newPart.SpawnBonuses(bonuses, _config.MaxBonusCountPerPart, _catalogDataRepository, _resourcesService);
            
            partGO.transform.position = _moveStrategy.GetNewPartPosition(newPart, _currentParts);
            partGO.SetActive(true);
        
            _currentParts.Add(newPart);
        }

        private void TryRemovePart(LevelPart part)
        {
            if (!_moveStrategy.CheckRemovePart(part)) 
                return;
        
            _currentParts.Remove(part);
            
            GameObject.Destroy(part.gameObject); 
        }

        private PartSpawnInfo NextPart()
        {
            _lastPartIndex = _lastPartIndex++ < _partInfos.Length - 1 ? _lastPartIndex : 0;
            return _partInfos[_lastPartIndex];
        }

        void IDisposable.Dispose()
        {
            IsInitialized = false;
        
            foreach (var part in _currentParts)
            {
                GameObject.Destroy(part); 
            }
            
            _partInfos = null;
            _currentParts = null;
        }

    }
}