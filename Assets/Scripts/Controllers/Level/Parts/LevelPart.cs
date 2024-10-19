using Controllers.Bonuses;
using Core.Data;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Services.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Level.Parts
{
    /// <summary>
    /// part which level consist of
    /// contains and spawn bonuses inside  
    /// </summary>
    public class LevelPart: MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform[] _bonusSpawnPoints;
        public Vector3 GetSize() => _renderer.bounds.size;

        public async UniTask SpawnBonuses(BonusSpawnInfo[] bonuses, int maxBonusCount, CatalogDataRepository catalogDataRepository,
            IResourcesService resourcesService)
        {
            var bonusCount = Random.Range(0, maxBonusCount + 1);
            if (bonusCount <= 0)
                return;

            for (int i = 0; i < bonusCount; i++)
            {
                var index = Random.Range(0, bonuses.Length);
                var bonusSpawnInfo = bonuses[index];

                var bonusData = catalogDataRepository.Bonuses.Get(bonusSpawnInfo.id);
                
                var spawnPointIndex = Random.Range(0, _bonusSpawnPoints.Length);
                var spawnPoint = _bonusSpawnPoints[spawnPointIndex];

                string assetKey = $"LevelCommon/Bonus{bonusData.Id}.prefab";
                var bonusGo = await resourcesService.Instantiate(
                    assetKey, 
                    spawnPoint.position, 
                    Quaternion.identity, 
                    spawnPoint.parent);
                
                var bonusController = bonusGo.GetComponent<BonusController>();
                bonusController.Setup(bonusSpawnInfo.id);
            }
        }
    }
}