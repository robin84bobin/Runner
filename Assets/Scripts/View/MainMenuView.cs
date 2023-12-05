using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Catalog;
using Services;
using Services.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class MainMenuView : MonoBehaviour
    {
        [Inject] private GameLevelService _gameLevelService;
        [Inject] public CatalogDataRepository CatalogDataRepository;
    
        [SerializeField] private Button startButtonTemplate;
        [SerializeField] private Transform buttonsContainer;
        private Button[] _startButtons;

        void Start()
        {
            CreateButtons();
        }

        private void CreateButtons()
        {
            List<LevelData> levels = CatalogDataRepository.Levels.GetAll().OrderBy(level => level.Id).ToList();
            int levelsCount = levels.Count;

            _startButtons = new Button[levelsCount];
            for (var index = 0; index < levels.Count; index++)
            {
                var levelData = levels[index];
                var button = Instantiate(startButtonTemplate, buttonsContainer);
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => StartClickHandler(levelData.Id));
                button.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {levelData.Id}";
                _startButtons[index] = button;
            }
        }

        private void StartClickHandler(string i)
        {
            LoadLevel(i);
        }

        private void LoadLevel(string levelId = "1")
        {
            _gameLevelService.StartLevel(levelId);
        }

        void OnDestroy()
        {
            foreach (var button in _startButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}
