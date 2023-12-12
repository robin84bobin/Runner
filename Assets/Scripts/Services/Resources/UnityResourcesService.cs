using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Services.Resources
{
    /// <summary>
    /// getting resources methods for unity resources
    /// </summary>
    public class UnityResourcesService : IResourcesService
    {
        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            SceneManager.LoadScene(sceneName, mode);
        }

        public async UniTask<string> LoadTextFile(string assetKey)
        {
            string text = string.Empty;
            if (assetKey.Contains("://"))
            {
                text = await LoadUrl(assetKey);
                return text;
            }

            await Task.Run(() =>
            {
                if (!IsFileExist(assetKey))
                    File.CreateText(assetKey).Close();
                text = File.ReadAllText(assetKey);
            });

            return text;
        }

        public async UniTask<T> LoadComponentFromPrefab<T>(string assetKey) where T:UnityEngine.Object
        {
            var gameObject = await LoadPrefab(assetKey);
            return gameObject.GetComponent<T>();
        }

        public async UniTask<GameObject> LoadPrefab(string assetKey)
        {
            var o = await UnityEngine.Resources.LoadAsync<GameObject>(assetKey).ToUniTask();
            return o as GameObject;
        }

        public async UniTask<T> LoadAsset<T>(string assetKey) where T :Object
        {
            var o = await UnityEngine.Resources.LoadAsync<T>(assetKey).ToUniTask();
            return o as T;
        }

        public async UniTask<GameObject> Instantiate(string assetKey, Vector3 position, Quaternion quaternion, Transform parent)
        {
            var go = await LoadPrefab(assetKey);
            return GameObject.Instantiate(go, position, quaternion, parent);
        }

        private bool IsFileExist(string path)
        {
            bool exists = File.Exists(path);
            if (!exists) {
                Debug.LogWarning("FILE NOT FOUND: " + path);
            }

            return exists;
        }
        
        private async Task<string> LoadUrl(string path)
        {
            var request = new UnityWebRequest(path);
            var operation =  request.SendWebRequest();

            while (!operation.isDone) 
                await Task.CompletedTask;
            
            return request.downloadHandler.text;
        } 
    }
}