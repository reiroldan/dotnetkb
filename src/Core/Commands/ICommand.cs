using System;

namespace DotNetKillboard.Commands
{

    /// <summary>
    /// Represents a command that can be sent
    /// </summary>
    public interface ICommand : IMessage
    {
        /// <summary>
        /// Aggregate root identifier
        /// </summary>
        Guid Id { get; set; }
    }
}