using Core.Common.Commands;
using Core.Data;
using Cysharp.Threading.Tasks;

namespace Startup.Startup
{
    /// <summary>
    /// 
    /// </summary>
    public class InitDataRepositoryCommand : Command
    {
        private BaseDataRepository _repository;

        public InitDataRepositoryCommand(BaseDataRepository repository)
        {
            _repository = repository;
        }
        
        public override async UniTask Execute()
        {
            _repository.OnInitComplete += OnInitComplete;
            _repository.OnInitProgress += SetProgress;
            await _repository.Init();
        }

        private void OnInitComplete()
        {
            _repository.OnInitProgress -= SetProgress;
            _repository.OnInitComplete -= OnInitComplete;
            Complete();
        }
    }
}