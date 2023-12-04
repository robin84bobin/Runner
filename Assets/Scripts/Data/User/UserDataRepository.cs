using Data.Catalog;
using Data.Proxy;
using Data.Repository;
using Services;
using UnityEngine;
using Zenject;

namespace Data.User
{
    public class UserDataRepository : Repository.BaseDataRepository, IFixedTickable
    {
        public const string CURRENCY = "currency";
        public const string SHOP = "shop";
        public const string PRODUCTS = "products";
        public const string FARM_ITEMS = "farmItems";
        public const string CELLS = "cells";
        public const string GRID = "grid";

        public  DataStorage<UserCurrency> Currency;

        public UserDataRepository(IDataProxyService dbProxyService, CatalogDataRepository catalogDataRepository,
            IResourcesService resourceService) : 
            base(dbProxyService) 
        {
            DataProxyService.SetupResourceService(resourceService);
            _catalogDataRepository = catalogDataRepository;
        }

        private CatalogDataRepository _catalogDataRepository;

        protected override void OnDataProxyInitialised()
        {
            bool userDataExist = DataProxyService.CheckSourceExist();
            if (userDataExist == false)
            {
                InitStartValuesFrom(_catalogDataRepository);
            }
        }

        protected override void CreateStorages()
        {
            Currency = CreateStorage<UserCurrency>(CURRENCY);
            // Currency = CreateStorage<UserCurrency>(CURRENCY);
            // Currency = CreateStorage<UserCurrency>(CURRENCY);
        }

        private void InitStartValuesFrom(CatalogDataRepository catalogData)
        {
            foreach (Currency currency in catalogData.Currency.GetAll())
            {
                UserCurrency c = new UserCurrency(){Type = currency.Type, CatalogDataId = currency.Id, Value = currency.Value};
                Currency.Set(c, currency.Id, true);
            }
            
            SaveAll();
        }

        private static bool _needSave;

        public static void Save()
        {
            _needSave = true;
        }

        private float deltaTime;

        public void FixedTick()
        {
            deltaTime += Time.fixedDeltaTime;
            if (deltaTime < 1f)
                return;
            deltaTime = 0f;
            
            if (_needSave)
            {
                _needSave = false;
                SaveAll();
            }
        }

        private void SaveAll()
        {
            Currency.SaveData();
            // ShopItems.SaveData();
            // FarmItems.SaveData();
            // Products.SaveData();
            // Cells.SaveData();
        }
    }
}