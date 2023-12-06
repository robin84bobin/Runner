﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Data.Catalog;
using Gameplay.Level.Parts.PartsMoving;
using Services;
using Services.GamePlay;
using UnityEngine;
using Zenject;

namespace Gameplay.Level.Parts
{
    public class LevelPartsController : ITickable, IDisposable
    {
        private readonly IResourcesService _resourcesService;
        private readonly GameCurrentLevelService _gameCurrentLevelService;
        private readonly ObjectPool _pool;
        private readonly IMoveLevelPartsStrategy _moveStrategy;
        private readonly ProjectConfig _config;
        private readonly CatalogDataRepository _catalogDataRepository;

        private Transform _container;
   
        private List<LevelPart> _parts;
        private List<LevelPart> _currentParts;

        int _lastPartIndex = -1;
        bool IsInitialized = false;

        //TODO use hero speed with modifiers
        private float MoveSpeed => _config.DefaultSpeed;

        public LevelPartsController(
            IResourcesService resourcesService, 
            GameCurrentLevelService gameCurrentLevelService, 
            ObjectPool pool,
            IMoveLevelPartsStrategy moveStrategy,
            ProjectConfig config,
            CatalogDataRepository catalogDataRepository
        )
        {
            _resourcesService = resourcesService;
            _gameCurrentLevelService = gameCurrentLevelService;
            _pool = pool;
            _moveStrategy = moveStrategy;
            _config = config;
            _catalogDataRepository = catalogDataRepository;
        }

        public async UniTask CreateParts(PartSpawnInfo[] partSpawnInfos, Transform container)
        {
            _container = container;
            _moveStrategy.Init(_container);
            _parts = new List<LevelPart>();
        
            foreach (var partSpawnInfo in partSpawnInfos)
            {
                var partPrefabName = _gameCurrentLevelService.GetPartPrefabName(partSpawnInfo.id);
            
                //TODO instantiate to pool
                var partGO = await _resourcesService.Instantiate( 
                                                                            partPrefabName, 
                                                                            Vector3.zero, Quaternion.identity, 
                                                                            _pool.containerObject.transform
                                                                            );
                LevelPart part = partGO.GetComponent<LevelPart>();
                _parts.Add(part);
                part.gameObject.SetActive(false);
            }
        
            _currentParts = new List<LevelPart>();

            for (int i = 0; i < _config.VisibleLevelPartCount; i++)
            {
                var part = NextPart();
                await SpawnPart(part);
            }

            IsInitialized = true;
        }

        public void Tick()
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

        private async UniTask SpawnPart(LevelPart part)
        {
            //TODO get from pool
            GameObject partGo = GameObject.Instantiate(part.gameObject, _container);
            LevelPart newPart = partGo.GetComponent<LevelPart>();

            //TODO move to factory?
            var bonuses = _gameCurrentLevelService.LevelData.bonuses;
            await newPart.Init(bonuses, _config.MaxBonusCountPerPart, _catalogDataRepository, _resourcesService);
            
            partGo.transform.position = _moveStrategy.GetNewPartPosition(newPart, _currentParts);
            partGo.transform.rotation = _container.rotation;
            partGo.SetActive(true);
        
            _currentParts.Add(newPart);
        }

        private void TryRemovePart(LevelPart part)
        {
            if (!_moveStrategy.CheckRemovePart(part)) 
                return;
        
            _currentParts.Remove(part);
            
            part.Release();
            GameObject.Destroy(part.gameObject); //TODO to pool 
        }

        private LevelPart NextPart()
        {
            _lastPartIndex = _lastPartIndex++ < _parts.Count - 1 ? _lastPartIndex : 0;
            return _parts[_lastPartIndex];
        }

        public void Dispose()
        {
            IsInitialized = false;
        
            foreach (var part in _parts)
            {
                part.Release();
                GameObject.Destroy(part); //TODO clear from pool  -> unload addressables assets 
            }
            _parts = null;
            _currentParts = null;
        }
    }
}