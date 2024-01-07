using System;
using System.Collections.Generic;
using System.IO;
using Core.Data.Repository;
using Cysharp.Threading.Tasks;
using InternalNewtonsoft.Json.Linq;
using Services.Resources;
using UnityEngine;

namespace Core.Data.Proxy
{
    /// <summary>
    /// get/save data to json file data source
    /// </summary>
    public class JsonDataProxyService: IDataProxyService
    {
        private IResourcesService _resourcesService;
        
        private string _path;

        private string _dataJson;
        private string _rootNode;

        public JsonDataProxyService(string path, string rootNode)
        {
            _rootNode = rootNode;
            _path =  path;
        }

        public void SetupResourceService(IResourcesService service)
        {
            _resourcesService = service;
        }
        
        public void Save<T>(string collection, T item, string id = "", Action<T> callback = null) where T : DataItem, new()
        {
            throw new NotImplementedException();
        }

        public bool CheckSourceExist() => File.Exists(_path);

        public event Action OnInitialized;

        public void Get<T>(string collection, Action<Dictionary<string, T>> callback, bool createIfNotExist = true) where T : DataItem, new()
        {
            TryRefreshData();

            if (string.IsNullOrEmpty(_dataJson) && createIfNotExist)
            {
                callback.Invoke(new Dictionary<string, T>());
                return;
            }

            JObject j = JObject.Parse(_dataJson);
            JToken jToken = j[_rootNode];
            if (string.IsNullOrEmpty(collection))
            {
                Debug.LogError(ToString() + " : no sourceName in storage had been set: " + collection);
                return ;
            }

            JToken jCollection = jToken.SelectToken(collection);
            if (jCollection == null && createIfNotExist)
            {
                callback.Invoke(new Dictionary<string, T>());
                Debug.LogWarning(ToString() + " : no source data was found by: " + collection);
                return ;
            }

            T[] items = jCollection.ToObject<T[]>();
            Dictionary<string, T> itemsDict = new Dictionary<string, T>();
            foreach (var item in items)
            {
                item.Type = collection;
                Debug.Log(collection + " : " + item.Id);
                itemsDict.Add(item.Id, item);
            }
            callback.Invoke(itemsDict);
        }

        public async UniTask<Dictionary<string, T>> Get<T>(string collection, bool createIfNotExist = true) where T : DataItem, new()
        {
            await TryRefreshData();

            if (string.IsNullOrEmpty(_dataJson) && createIfNotExist)
            {
                return new Dictionary<string, T>();
            }

            JObject j = JObject.Parse(_dataJson);
            JToken jToken = j[_rootNode];
            if (string.IsNullOrEmpty(collection))
            {
                Debug.LogError(ToString() + " : no sourceName in storage had been set: " + collection);
                return new Dictionary<string, T>();
            }

            JToken jCollection = jToken.SelectToken(collection);
            if (jCollection == null && createIfNotExist)
            {
                Debug.LogWarning(ToString() + " : no source data was found by: " + collection);
                return new Dictionary<string, T>();
            }

            T[] items = jCollection.ToObject<T[]>();
            Dictionary<string, T> itemsDict = new Dictionary<string, T>();
            
            foreach (var item in items)
            {
                item.Type = collection;
                Debug.Log(collection + " : " + item.Id);
                if (!itemsDict.TryAdd(item.Id, item))
                {
                    Debug.LogError($"Cant add {item.Type}, Id={item.Id} to data storage. Id already exist");
                }
            }
            return itemsDict;
        }

        public async UniTask Init()
        {
            await TryRefreshData();
            if (OnInitialized != null)
                OnInitialized.Invoke();
        }

        private async UniTask TryRefreshData()
        {
            _dataJson = await _resourcesService.LoadTextFile(_path);
        }

        public void SaveCollection<T>(string collection, Dictionary<string, T> items, Action callback = null)
            where T : DataItem, new()
        {
            TryRefreshData();

            JObject j = string.IsNullOrEmpty(_dataJson)?
                new JObject(): 
                JObject.Parse(_dataJson);
            
            if (j[_rootNode] == null)
                j[_rootNode] = new JObject();

            var jItems = JToken.FromObject(new List<T>(items.Values));
            j[_rootNode][collection] = jItems;

            StreamWriter writer = File.CreateText(_path);
            var sourceString = j.ToString();
            writer.Write(sourceString.ToCharArray());
            writer.Close();
        }

        public T GetConfigObject<T>(string name)
        {
            JObject j = JObject.Parse(_dataJson);
            JToken jToken = j[_rootNode];
            return jToken[name].ToObject<T>();
        }
    }
}