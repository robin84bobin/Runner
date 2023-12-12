using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Services.Resources
{
    /// <summary>
    /// getting resources methods for Addressables system
    /// </summary>
    public class AddressablesResourcesService : IResourcesService
    {
        public async void LoadScene(string sceneName, LoadSceneMode mode)
        {
            var handle = Addressables.LoadSceneAsync(sceneName, mode);
            await handle.Task;
        }

        public async UniTask<string> LoadTextFile(string assetKey)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(assetKey);
            var t =  await handle.Task;
            return t.text;
        }

        public async UniTask<T> LoadComponentFromPrefab<T>(string assetKey) where T:Object
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetKey);
            var gameObject =  await handle.Task;
            var component = gameObject.GetComponent<T>();
            return component;
        }

        public async UniTask<GameObject> LoadPrefab(string assetKey)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetKey);
            var gameObject =  await handle.Task;
            return gameObject;
        }

        public async UniTask<T> LoadAsset<T>(string assetKey) where T :Object
        {
            var handle = Addressables.LoadAssetAsync<T>(assetKey);
            T asset =  await handle.Task;
            return asset;
        }

        public async UniTask<GameObject> Instantiate(string assetKey, Vector3 position, Quaternion quaternion, Transform parent)
        {
            var instantiationParameters = new InstantiationParameters(position, quaternion, parent);
            return await Addressables.InstantiateAsync(assetKey, instantiationParameters).ToUniTask();
        }
    }
}