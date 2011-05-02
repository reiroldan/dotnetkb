using System;

namespace DotNetKillboard.Commands
{

    /// <summary>
    /// Base type for commands
    /// </summary>
    public class CommandBase : ICommand
    {
        public Guid Id { get; set; }
        
        public CommandBase(Guid id) {
            Id = id;
        }
    }
}