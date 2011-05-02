namespace DotNetKillboard.Events
{

    /// <summary>
    /// Base class for synchroneous events
    /// </summary>
    public class EventBase : IEvent
    {
        public int Version { get; set; }

        public bool Async { get { return false; } }
    }
}