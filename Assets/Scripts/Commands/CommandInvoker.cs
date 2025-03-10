using System.Collections.Generic;
using Interfaces;

namespace Commands
{
    public class CommandInvoker
    {
        private readonly Stack<ITileCommand> _commandHistory = new Stack<ITileCommand>();

        public void ExecuteCommand(ITileCommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

       
    }
}