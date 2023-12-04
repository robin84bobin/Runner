using Cysharp.Threading.Tasks;
using Data.Repository;

namespace Commands.Startup
{
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