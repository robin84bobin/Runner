using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Services;
using Services.GamePlay;
using Zenject;

public class LevelPartsController : ITickable, IInitializable, IDisposable
{
    private readonly IResourcesService _resourcesService;
    private readonly GameLevelService _gameLevelService;
    private readonly ObjectPool _pool;
    private readonly IMoveLevelPartsStrategy _moveStrategy;

    private Transform _container;
    
    private Vector3 _partRemoveBound;
   
    //TODO move to config?
    private readonly float _speed = -0.01f;
    private readonly int _partCnt = 3;
    private float _partSize = 24;

    private List<LevelPart> _parts;
    private List<LevelPart> _currentParts;

    float _lastPartPos = 0f;
    int _lastPartIndex = -1;
    
    bool IsInitialised = false;


    //TODO inject vertical move parts strategy
    public LevelPartsController(
        IResourcesService resourcesService, 
        GameLevelService gameLevelService, 
        ObjectPool pool,
        IMoveLevelPartsStrategy moveStrategy
        )
    {
        _resourcesService = resourcesService;
        _gameLevelService = gameLevelService;
        _pool = pool;
        _moveStrategy = moveStrategy;
    }

    public async UniTask CreateParts(PartSpawnInfo[] partSpawnInfos, Transform container)
    {
        _container = container;
        _moveStrategy.Init(_container);
        _parts = new List<LevelPart>();
        
        foreach (var partSpawnInfo in partSpawnInfos)
        {
            var partPrefabName = _gameLevelService.GetPartPrefabName(partSpawnInfo.id);
            
            //TODO instantiate to pool
            var partGO = await _resourcesService.Instantiate( partPrefabName, 
                                                                        Vector3.zero, Quaternion.identity, 
                                                                        _pool.containerObject.transform
                                                                        );
            LevelPart part = partGO.GetComponent<LevelPart>();
            _parts.Add(part);
            part.gameObject.SetActive(false);
        }
        
        _currentParts = new List<LevelPart>();

        for (int i = 0; i < _partCnt; i++)
        {
            var part = NextPart();
            SpawnPart(part);
        }

        IsInitialised = true;
    }

    private void SpawnPart(LevelPart part)
    {
       //TODO get from pool
        GameObject partGo = GameObject.Instantiate(part.gameObject, _container);
        LevelPart newPart = partGo.GetComponent<LevelPart>();

        partGo.transform.position = _moveStrategy.GetNewPartPosition(newPart, _currentParts);
        partGo.transform.rotation = _container.rotation;
        partGo.SetActive(true);
        
        _currentParts.Add(newPart);
    }

    public void Tick()
    {
        if (!IsInitialised) 
            return;

        for (int i = 0; i < _currentParts.Count; i++)
        {
            var part = _currentParts[i];
            _moveStrategy.Move(part,_speed);
            TryRemovePart(part);
        }

        TryAddPart();
    }

    private void TryAddPart()
    {
        if (_currentParts.Count < _partCnt)
        {
            var part = NextPart();
            SpawnPart(part);
        }
    }
    
    private void TryRemovePart(LevelPart part)
    {
        if (_moveStrategy.CheckRemovePart(part))
        {
            _currentParts.Remove(part);
            GameObject.Destroy(part.gameObject); //TODO to pool 
        }
    }

    private LevelPart NextPart()
    {
        _lastPartIndex = _lastPartIndex++ < _parts.Count - 1 ? _lastPartIndex : 0;
        return _parts[_lastPartIndex];
    }

    public void Dispose()
    {
        IsInitialised = false;
        
        foreach (var part in _parts)
        {
            GameObject.Destroy(part); //TODO clear from pool  -> unload addressables assets 
        }
        _parts = null;
        _currentParts = null;
    }

    public void Initialize()
    {
        //TODO
    }
}