using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Resources
{
    /// <summary>
    /// interface for getting resources methods
    /// </summary>
    public interface IResourcesService
    {
        void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        UniTask<string> LoadTextFile(string assetKey);
        UniTask<T> LoadComponentFromPrefab<T>(string assetKey) where T:UnityEngine.Object;
        UniTask<GameObject> LoadPrefab(string assetKey);
        UniTask<T> LoadAsset<T>(string assetKey) where T :Object;
        UniTask<GameObject> Instantiate(string assetKey, Vector3 position, Quaternion quaternion, Transform parent);
    }
}