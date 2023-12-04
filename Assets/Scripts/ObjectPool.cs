using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PoolData
{
    public GameObject prefab;
    public int poolAmount;
}

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool instance;

    public PoolData[] poolDataList;

    private Dictionary<string, List<GameObject>> _poolObjectsMap;
    private Dictionary<string, GameObject> _objectsMap;

    public GameObject[] objectPrefabs;

    public List<GameObject>[] pooledObjects;

    public int[] amountToBuffer;

    public int defaultBufferAmount = 5;

    protected GameObject containerObject;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        containerObject = new GameObject("ObjectPool");
        containerObject.transform.SetParent( gameObject.transform);
        DontDestroyOnLoad(containerObject);

        initPoolMap();
     }

    private void initPoolMap()
    {
        //create pool map
        _poolObjectsMap = new Dictionary<string, List<GameObject>>();
        _objectsMap = new Dictionary<string, GameObject>();

        foreach (var data in poolDataList)
        {
            if (data.prefab == null)
            {
                continue;
            }

            _poolObjectsMap.Add(data.prefab.name, new List<GameObject>());
            _objectsMap.Add(data.prefab.name, data.prefab);

            int amount = data.poolAmount > 0 ? data.poolAmount : defaultBufferAmount;
            for (int i = 0; i < amount; i++)
            {
                GameObject newObj = Instantiate(data.prefab) as GameObject;
                newObj.name = data.prefab.name;
                PoolObject(newObj);
            }
        }
    }


    public GameObject GetObject(string objectType, bool onlyPooled = false)
    {

        if (_poolObjectsMap.ContainsKey(objectType))
        {
            if (_poolObjectsMap[objectType].Count > 0)
            {
                GameObject pooledObject = _poolObjectsMap[objectType][0];
                _poolObjectsMap[objectType].RemoveAt(0);
                pooledObject.transform.parent = null;
                pooledObject.SetActive(true);
                pooledObject.SendMessage("OnGetFromPool", SendMessageOptions.DontRequireReceiver);
                return pooledObject;
            }
            else if (!onlyPooled)
            {
                GameObject go = Instantiate(_objectsMap[objectType]) as GameObject;
                go.SendMessage("OnGetFromPool", SendMessageOptions.DontRequireReceiver);
                return go;
            }
           
        }

        return null;
    }

    public bool PoolObject(GameObject obj, bool destroyIfNotPooled = true)
    {
        if (_poolObjectsMap.ContainsKey(obj.name))
        {
            obj.SetActive(false);
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
