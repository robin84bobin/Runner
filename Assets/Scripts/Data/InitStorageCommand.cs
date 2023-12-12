using Common.Commands;
using Cysharp.Threading.Tasks;
using Data.Proxy;
using Data.Repository;
using UnityEngine;

namespace Data
{

    /// <summary>
    /// loads data items from data proxy to data storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InitStorageCommand<T> : Command where T : DataItem, new()
    {
        private IDataProxyService _dataProxyService;
        private DataStorage<T> _storage;

        public InitStorageCommand(DataStorage<T> storage, IDataProxyService dataProxyService)
        {
            _dataProxyService = dataProxyService;
            _storage = storage;
        }

        public override async UniTask Execute()
        {
            Debug.Log(this + " --> " + _storage.CollectionName);
            
            var items = await _dataProxyService.Get<T>(_storage.CollectionName);
            _storage.SetData(items);
            Complete();
        }

        protected override void Release()
        {
            base.Release();
            _storage = null;
        }
    }
}