using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Services;
using Services.GamePlay;
using Zenject;

public class ScrollController : ITickable, IInitializable, IDisposable
{
    private readonly IResourcesService _resourcesService;
    private readonly GameLevelService _gameLevelService;
    private readonly ObjectPool _pool;

    private Transform _container;
    private Vector3 _removeBound = new (0f, -25f, 0f);
   
    //TODO move to config?
    private readonly float _speed = -0.01f;
    private readonly int _partCnt = 3;
    private float _partSize = 24;

    private List<ScrollablePart> _parts;
    private List<ScrollablePart> _currentParts;

    float _lastPartPos = 0f;
    int _lastPartIndex = -1;
    
    bool IsInitialised = false;


    public ScrollController(IResourcesService resourcesService, GameLevelService gameLevelService, ObjectPool pool)
    {
        _resourcesService = resourcesService;
        _gameLevelService = gameLevelService;
        _pool = pool;
    }

    public async UniTask CreateParts(PartSpawnInfo[] partSpawnInfos, Transform container)
    {
        _container = container;
        
        _parts = new List<ScrollablePart>();
        foreach (var partSpawnInfo in partSpawnInfos)
        {
            var partPrefabName = _gameLevelService.GetPartPrefabName(partSpawnInfo.id);
            
            //TODO instantiate to pool
            var partGO = await _resourcesService.Instantiate( partPrefabName, 
                                                                        Vector3.zero, 
                                                                        Quaternion.identity, 
                                                                        _pool.containerObject.transform
                                                                        );
            ScrollablePart part = partGO.GetComponent<ScrollablePart>();
            _parts.Add(part);
            part.gameObject.SetActive(false);
        }
        
        _currentParts = new List<ScrollablePart>();

        for (int i = 0; i < _partCnt; i++)
        {
            var part = NextPart();
            SpawnPart(part);
        }

        IsInitialised = true;
    }

    private void SpawnPart(ScrollablePart part)
    {
        float nextPartPos = _container.position.y;
        
        if (_currentParts.Count > 0)
        {
            var lastPart = _currentParts[_currentParts.Count - 1];
            _lastPartPos = lastPart.transform.position.y;
            nextPartPos = _lastPartPos + _partSize;
        }

        //TODO get from pool
        GameObject partGo = GameObject.Instantiate(part.gameObject, _container);
        partGo.transform.position = new Vector3(_container.position.x, nextPartPos, _container.position.z); ;
        partGo.transform.rotation = _container.rotation;
        partGo.SetActive(true);
        
        ScrollablePart newPart = partGo.GetComponent<ScrollablePart>();
        newPart.Init();
        
        _currentParts.Add(newPart);
    }

    public void Tick()
    {
        if (!IsInitialised) 
            return;

        for (int i = 0; i < _currentParts.Count; i++)
        {
            var part = _currentParts[i];
            part.Move(_speed);
            CheckRemovePart(part);
        }

        if (_currentParts.Count < _partCnt)
        {
            AddPart();
        }
    }

    private void CheckRemovePart(ScrollablePart part)
    {
        if (part.transform.position.y < _removeBound.y)
        {
            _currentParts.Remove(part);
            GameObject.Destroy(part.gameObject);
        }
    }

    private void AddPart()
    {
        var part = NextPart();
        SpawnPart(part);
    }

    ScrollablePart NextPart()
    {
        _lastPartIndex = _lastPartIndex++ < _parts.Count - 1 ? _lastPartIndex : 0;
        return _parts[_lastPartIndex];
    }

    public void Dispose()
    {
        IsInitialised = false;
        
        foreach (var part in _parts)
        {
            //TODO clear from pool  -> unload addressables assets 
            GameObject.Destroy(part);
        }
        _parts = null;
        _currentParts = null;
    }

    public void Initialize()
    {
        //TODO
    }
}
