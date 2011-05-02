namespace DotNetKillboard.Commands
{

    /// <summary>
    /// Handles a command
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handle command
        /// </summary>
        /// <param name="command"></param>
        void Handle(TCommand command);
    }
}