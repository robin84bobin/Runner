using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public interface IResourcesService
    {
        void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        UniTask<string> LoadTextFile(string path);
        UniTask<T> LoadComponentFromPrefab<T>(string path) where T:UnityEngine.Object;
        UniTask<GameObject> LoadPrefab(string path);
        UniTask<T> LoadAsset<T>(string path) where T :Object;
        UniTask<GameObject> Instantiate(string prefabName, Vector3 position, Quaternion quaternion, Transform parent);
    }
}