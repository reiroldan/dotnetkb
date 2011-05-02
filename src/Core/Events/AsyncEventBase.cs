namespace DotNetKillboard.Events
{

    /// <summary>
    /// Base class for asynchroneous events
    /// </summary>
    public class AsyncEventBase : IEvent
    {
        public int Version { get; set; }

        public bool Async {
            get { return true; }
        }
    }
}