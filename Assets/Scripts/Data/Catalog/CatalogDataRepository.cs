using Data.Proxy;
using Data.Repository;
using Services;
using Unity.VisualScripting;

namespace Data.Catalog
{
    public class CatalogDataRepository : BaseDataRepository
    {
        public DataStorage<LevelData> Levels;
        public DataStorage<BonusData> Bonuses;
        public DataStorage<AbilityData> Abilities;
        
        public CatalogDataRepository(IDataProxyService dataProxyService, IResourcesService resourceService) : 
            base(dataProxyService)
        {
            DataProxyService.SetupResourceService(resourceService);
        }

        protected override void CreateStorages()
        {
            Levels = CreateStorage<LevelData>("Levels");
            Bonuses = CreateStorage<BonusData>("Bonuses");
            Abilities = CreateStorage<AbilityData>("Abilities");
        }
    }
}