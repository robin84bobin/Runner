using Data.Proxy;
using Data.Repository;
using Services;

namespace Data.Catalog
{
    public class CatalogDataRepository : BaseDataRepository
    {
        public DataStorage<Currency> Currency;
        public DataStorage<ShopItem> ShopItems;
        public DataStorage<Product> Products;
        public DataStorage<FarmItem> FarmItems;
        
        public CatalogDataRepository(IDataProxyService dataProxyService, IResourcesService resourceService) : 
            base(dataProxyService)
        {
            DataProxyService.SetupResourceService(resourceService);
        }

        protected override void CreateStorages()
        {
            Currency = CreateStorage<Currency>("currency");
            ShopItems = CreateStorage<ShopItem>("shop");
            Products = CreateStorage<Product>("products");
            FarmItems = CreateStorage<FarmItem>("farmItems");
        }
    }
}