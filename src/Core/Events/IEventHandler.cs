namespace DotNetKillboard.Events
{

    /// <summary>
    /// Represents an event handler
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Handle the specified event type
        /// </summary>
        /// <param name="e"></param>
        void Handle(TEvent e);
    }
}