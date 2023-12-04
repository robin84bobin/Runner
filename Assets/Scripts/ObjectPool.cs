using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class PoolData
{
    public PoolableBehaviour prefab;
    public int poolAmount;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject containerObject;
    [SerializeField] private PoolData[] poolDataList;

    private Dictionary<string, List<PoolableBehaviour>> _poolObjectsMap;
    private Dictionary<string, PoolableBehaviour> _objectsMap;

    public GameObject[] objectPrefabs;

    public List<PoolableBehaviour>[] pooledObjects;

    public int[] amountToBuffer;

    public int defaultBufferAmount = 5;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitPoolMap();
     }

    private void InitPoolMap()
    {
        //create pool map
        _poolObjectsMap = new Dictionary<string, List<PoolableBehaviour>>();
        _objectsMap = new Dictionary<string, PoolableBehaviour>();

        foreach (var data in poolDataList)
        {
            if (data.prefab == null)
                continue;

            _poolObjectsMap.Add(data.prefab.name, new List<PoolableBehaviour>());
            _objectsMap.Add(data.prefab.name, data.prefab);

            int amount = data.poolAmount > 0 ? data.poolAmount : defaultBufferAmount;
            for (int i = 0; i < amount; i++)
            {
                PoolableBehaviour newObj = Instantiate(data.prefab);
                newObj.name = data.prefab.name;
                PoolObject(newObj);
            }
        }
    }


    public PoolableBehaviour GetObject(string objectType, bool onlyPooled = false)
    {
        if (_poolObjectsMap.ContainsKey(objectType))
        {
            if (_poolObjectsMap[objectType].Count > 0)
            {
                PoolableBehaviour pooledObject = _poolObjectsMap[objectType][0];
                _poolObjectsMap[objectType].RemoveAt(0);
                pooledObject.transform.parent = null;
                pooledObject.gameObject.SetActive(true);
                pooledObject.OnGetFromPool();
                return pooledObject;
            }

            if (!onlyPooled)
            {
                PoolableBehaviour go = Instantiate(_objectsMap[objectType]) as PoolableBehaviour;
                go.OnGetFromPool();
                return go;
            }
           
        }

        return null;
    }

    public bool PoolObject(PoolableBehaviour obj, bool destroyIfNotPooled = true)
    {
        if (_poolObjectsMap.ContainsKey(obj.name))
        {
            obj.gameObject.SetActive(false);
            obj.transform.parent = containerObject.transform;
            _poolObjectsMap[obj.name].Add(obj);
            return true;
        }

        if (destroyIfNotPooled)
        {
            Destroy(obj);
        }
        return false;
    }

}

public class PoolableBehaviour : MonoBehaviour
{
    public void OnGetFromPool()
    {
        
    }
}
