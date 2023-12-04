using Common;
using Services;
using Services.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class MainMenuView : MonoBehaviour
    {
        [Inject] private GameplayLevelService GameplayLevelService;
        [Inject] public IResourcesService ResourcesService;
    
        [SerializeField] private Button _startButton1;
        [SerializeField] private Button _startButton2;
        [SerializeField] private Button _startButton3;

        void Start()
        {
            _startButton1.onClick.AddListener(() => StartClickHandler(1));
            _startButton2.onClick.AddListener(() => StartClickHandler(2));
            _startButton3.onClick.AddListener(() => StartClickHandler(3));
        }

        private void StartClickHandler(int i)
        {
            LoadLevel(i);
        }

        private void LoadLevel(int levelId = 1)
        {
            GameplayLevelService.SetupLevel(levelId);
            ResourcesService.LoadScene(AppConstants.Scenes.Game);
            
            var levelSceneName = AppConstants.Scenes.Level + levelId;
            ResourcesService.LoadScene(levelSceneName, LoadSceneMode.Additive);
        }

        void OnDestroy()
        {
            _startButton1.onClick.RemoveAllListeners();
            _startButton2.onClick.RemoveAllListeners();
            _startButton3.onClick.RemoveAllListeners();
        }
    }
}
