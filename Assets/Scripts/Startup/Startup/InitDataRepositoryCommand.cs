using Common.Commands;
using Cysharp.Threading.Tasks;
using Data.Repository;

namespace Startup.Startup
{
    /// <summary>
    /// 
    /// </summary>
    public class InitDataRepositoryCommand : Command
    {
        private BaseDataRepository _baseDataRepository;

        public InitDataRepositoryCommand(BaseDataRepository baseDataRepository)
        {
            _baseDataRepository = baseDataRepository;
        }
        
        public override async UniTask Execute()
        {
            _baseDataRepository.OnInitComplete += OnInitComplete;
            _baseDataRepository.OnInitProgress += SetProgress;
            await _baseDataRepository.Init();
        }

        private void OnInitComplete()
        {
            _baseDataRepository.OnInitProgress -= SetProgress;
            _baseDataRepository.OnInitComplete -= OnInitComplete;
            Complete();
        }
    }
}