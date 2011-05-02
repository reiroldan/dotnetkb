namespace DotNetKillboard.Events
{
    /// <summary>
    /// Represents an event which has already occured
    /// </summary>
    public interface IEvent : IMessage
    {
        /// <summary>
        /// Version of this event
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Should this event be handled in an Asynchroneous fashion
        /// </summary>
        bool Async { get; }
    }
}