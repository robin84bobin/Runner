using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Services;
using Services.GamePlay;
using Zenject;

public class ScrollController : ITickable
{
    private readonly IResourcesService _resourcesService;
    private readonly GameLevelService _gameLevelService;
    private readonly ObjectPool _pool;

    private Transform _container;
    private Vector3 _removeBound = new Vector3(-15, 0f, 0f);
   
    //move to config?
    private readonly float _speed = -0.01f;
    private readonly int _partCnt = 3;
    private float _partSize;

    private List<ScrollablePart> _parts;
    private List<ScrollablePart> _currentParts;

    float _lastPartPos = 0f;
    int _lastPartIndex = -1;
    
    bool start = false;


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
            var partGO = await _resourcesService.Instantiate(partPrefabName, container.position, container.rotation, container);
            ScrollablePart part = partGO.GetComponent<ScrollablePart>();
            _parts.Add(part);
        }
        
        _currentParts = new List<ScrollablePart>();

        for (int i = 0; i < _partCnt; i++)
        {
            var part = NextPart();
            CreatePart(part);
        }

        start = true;
    }

    private void CreatePart(ScrollablePart part)
    {
        float nextPartPos = _container.position.x;
        if (_currentParts.Count > 0)
        {
            var lastPart = _currentParts[_currentParts.Count - 1];
            _lastPartPos = lastPart.transform.position.x;
            nextPartPos = _lastPartPos + _partSize;
        }

        GameObject partGo = GameObject.Instantiate(part.gameObject);
        partGo.transform.position = new Vector3(nextPartPos, _container.position.y, _container.position.z); ;
        partGo.transform.rotation = Quaternion.identity;

        ScrollablePart newPart = partGo.GetComponent<ScrollablePart>();
        newPart.Init();
        _currentParts.Add(newPart);

    }

    public void Tick()
    {
        if (!start) 
            return;

        for (int i = 0; i < _currentParts.Count; i++)
        {
            var part = _currentParts[i];
            part.Move(Time.deltaTime / _speed);
            CheckRemovePart(part);
        }

        if (_currentParts.Count < _partCnt)
        {
            AddPart();
        }
    }

    private void CheckRemovePart(ScrollablePart part)
    {
        if (part.transform.position.x < _removeBound.x)
        {
            _currentParts.Remove(part);
            GameObject.Destroy(part.gameObject);
        }
    }

    private void AddPart()
    {
        var part = NextPart();
        CreatePart(part);
    }

    ScrollablePart NextPart()
    {
        _lastPartIndex = _lastPartIndex++ < _parts.Count - 1 ? _lastPartIndex : 0;
        return _parts[_lastPartIndex];
    }
}
