using System;
using System.Collections.Generic;
using Commands;
using Commands.Data;
using Cysharp.Threading.Tasks;
using Data.Proxy;

namespace Data.Repository
{
    public abstract class BaseDataRepository 
    {
        public List<Command> InitStoragesCommands { get; }
        private readonly Dictionary<string, IDataStorage> _storages = new Dictionary<string, IDataStorage>();
        public event Action<float> OnInitProgress;
        public event Action OnInitComplete = delegate { };

        protected IDataProxyService DataProxyService;


        protected BaseDataRepository(IDataProxyService dataProxyService)
        {
            DataProxyService = dataProxyService;
            InitStoragesCommands = new List<Command>();
        }

        public async UniTask Init()
        {
            //TODO initialize data proxy by separate command outside of repository
            
            DataProxyService.OnInitialized += OnDataInitComplete;
            await DataProxyService.Init();
        }

        private void OnDataInitComplete()
        {
            DataProxyService.OnInitialized -= OnDataInitComplete;
            
            CreateStorages();
            InitStorages();
            OnDataProxyInitialised();
        }

        protected abstract void CreateStorages();


        protected DataStorage<T> CreateStorage<T>(string collectionName) where T : DataItem, new()
        {
            var dataStorage = new DataStorage<T>(collectionName, DataProxyService);
            _storages.Add(collectionName, dataStorage);
            
            InitStorageCommand<T> command = new InitStorageCommand<T>(dataStorage, DataProxyService);
            InitStoragesCommands.Add(command);
            
            return dataStorage;
        }

        protected virtual void OnDataProxyInitialised(){}

        private void InitStorages()
        {
            CommandSerialSequence sequence = new CommandSerialSequence(InitStoragesCommands.ToArray());
            sequence.OnProgress += OnInitProgress;
            sequence.OnComplete += () =>
            {
                OnInitComplete.Invoke();
            };
            sequence.Execute();
        }

        public T GetSetting<T>(string name)
        {
            return DataProxyService.GetConfigObject<T>(name);
        }
    }
}