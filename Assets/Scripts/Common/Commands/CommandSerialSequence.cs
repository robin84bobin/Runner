using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ModestTree;

namespace Common.Commands
{
    /// <summary>
    /// Executes commands in serial mode - one by one
    /// </summary>
    public class CommandSerialSequence : Command
    {
        protected Queue<Command> _queue;
        private int _commandsCount;
        private int _commandsCompleted;

        public CommandSerialSequence(params Command[] commands)
        {
            _queue = new Queue<Command>(commands);
        }
        
        public override async UniTask Execute()
        {
            _commandsCount = _queue.Count;
            _commandsCompleted = 0;

            if (_queue.IsEmpty())
            {
                Complete();
                return;
            }
            
            await ExecuteNextCommand();
        }

        private async UniTask ExecuteNextCommand()
        {
            var command = _queue.Dequeue();
            command.OnComplete += OnCommandComplete;
            command.OnProgress += OnCommandProgress;
            await command.Execute();
        }

        private void OnCommandProgress(float percent)
        {
            var percentPerCommand = 1f / _commandsCount;
            var completedPercent = percentPerCommand * _commandsCompleted;
            float value = completedPercent + percentPerCommand * percent;
            SetProgress(value);
        }

        private void OnCommandComplete()
        {
            _commandsCompleted++;
            
            if (_queue.Count == 0)
            {
                Complete();
                return;
            }
            
            ExecuteNextCommand();
        }
    }
}